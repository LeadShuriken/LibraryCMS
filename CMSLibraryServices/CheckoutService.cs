using System;
using System.Collections.Generic;
using CMSLibraryData;
using CMSLibraryData.DBModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMSLibraryServices
{
    /// <summary>
    /// CheckoutServices
    /// </summary>
    public class CheckoutService : AssetsBase , ICheckout
    {
        private CMSLibraryContext _context;

        public CheckoutService(CMSLibraryContext context)
             : base(context) // context passed to AssetsBasse
        {
            _context = context;
        }

        /// <summary>
        /// Getting a checkout by id
        /// </summary>
        /// <param name="id"></param>
        public Checkout Get(int id)
        {
            return _context.Checkout.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Getting all checkouts
        /// </summary>
        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkout;
        }

        /// <summary>
        /// Checks out an CMSLibraryAsset to a library card id and fills the entry in history
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="libraryCardId"></param>
        public string CheckoutItem(int assetId, int libraryCardId)
        {
            if (IsCheckedOut(assetId))
            {
                return "Double Checked Out!"; // don't double checkout
            }

            DateTime now = DateTime.Now;
            CMSLibraryAsset item = _context.CMSLibraryAsset
                .Include(a => a.Status)
                .FirstOrDefault(a => a.Id == assetId);

            UpdateStatus(assetId, "Checked Out"); // Changes the status of the asset

            CMSLibraryCard libraryCard = _context.CMSLibraryCard
                .Include(c => c.Checkouts)
                .FirstOrDefault(a => a.Id == libraryCardId);

            if (libraryCard == null)
            {
                return "Invalid card!"; // make sure the library card is valid
            }

            Checkout checkout = new Checkout // creating the checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now, assetId)
            };

            _context.Add(checkout);

            CheckoutHistory checkoutHistory = new CheckoutHistory // creating the checkout history
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
            return null;
        }

        /// <summary>
        /// Removes all placed holds on the item
        /// </summary>
        /// <param name="assetId"></param>
        public void RemoveHolds(int assetId)
        {
            IQueryable<Hold> currentHolds = _context.Hold
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == assetId);

            if (currentHolds != null)
            {
                _context.RemoveRange(currentHolds);
            }
        }

        /// <summary>
        /// Marks asset as lost from the view
        /// </summary>
        /// <param name="assetId"></param>
        public void MarkLost(int assetId)
        {
            DateTime now = DateTime.Now;
            UpdateStatus(assetId, "Lost");

            RemoveHolds(assetId);
            RemoveExistingCheckout(assetId);
            CloseExistingCheckoutHistory(assetId, now);
            _context.SaveChanges();
        }

        /// <summary>
        /// Marks asset as found from the view
        /// </summary>
        /// <param name="assetId"></param>
        public void MarkFound(int assetId)
        {
            UpdateStatus(assetId, "Available");
            _context.SaveChanges();
        }

        /// <summary>
        /// Util method updating the Status of the asset
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="v"></param>
        private void UpdateStatus(int assetId, string v)
        {
            CMSLibraryAsset item = _context.CMSLibraryAsset
                .FirstOrDefault(a => a.Id == assetId);
            _context.Update(item);
            item.Status = _context.Status.FirstOrDefault(a => a.Name == v);
        }

        /// <summary>
        /// Closes the element checkout history with the current time (checks in)
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="now"></param>
        private void CloseExistingCheckoutHistory(int assetId, DateTime now)
        {
            // close any existing checkout history
            CheckoutHistory history = _context.CheckoutHistory
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.LibraryAsset.Id == assetId && h.CheckedIn == null);

            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        /// <summary>
        /// Removes the existing checkouts
        /// </summary>
        /// <param name="assetId"></param>
        private void RemoveExistingCheckout(int assetId)
        {
            Checkout checkout = _context.Checkout
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.LibraryAsset.Id == assetId);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        /// <summary>
        /// Places a Hold to the item linked to a card id and subscriber
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="libraryCardId"></param>
        /// <returns></returns>
        public string PlaceHold(int assetId, int libraryCardId)
        {
            DateTime now = DateTime.Now;

            CMSLibraryAsset asset = _context.CMSLibraryAsset
                .Include(a => a.Status)
                .FirstOrDefault(a => a.Id == assetId);

            CMSLibraryCard libraryCard = _context.CMSLibraryCard
                .FirstOrDefault(a => a.Id == libraryCardId);

            IEnumerable<Hold> currentHolds = GetCurrentHolds(assetId);
            Checkout currentCheckout = GetLatestCheckout(assetId);
            if (libraryCard == null) // when the card is not valid
            {
                return "Invalid card!";
            }
            else if (currentCheckout.LibraryCard.Id == libraryCardId) // when the checkouter is trying to place a hold
            {
                return "You cannot place a hold on an item which you have checked out!";
            }
            else if (currentHolds.Any(a => a.LibraryCard.Id == libraryCardId)) // when you are trying to place a second hold on an item
            {
                return "You have allready placed a hold on that item!";
            }

            Hold hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = libraryCard
            };

            _context.Add(hold);
            _context.SaveChanges();
            return null;
        }

        /// <summary>
        /// Checking in an item
        /// </summary>
        /// <param name="assetId"></param>
        public void CheckInItem(int assetId)
        {
            DateTime now = DateTime.Now;

            CMSLibraryAsset item = _context.CMSLibraryAsset
                .FirstOrDefault(a => a.Id == assetId);

            _context.Update(item);

            RemoveExistingCheckout(assetId);

            CloseExistingCheckoutHistory(assetId, now);
            
            IQueryable<Hold> currentHolds = _context.Hold
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == assetId);

            // if there are current holds, check out the item to the earliest
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);
                return;
            }

            // otherwise, set item status to available
            UpdateStatus(assetId, "Available");

            _context.SaveChanges();
        }

        /// <summary>
        /// Checking the item in to the earliest subscriber to have placed a hold
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="currentHolds"></param>
        private void CheckoutToEarliestHold(int assetId, IEnumerable<Hold> currentHolds)
        {
            Hold earliestHold = currentHolds.OrderBy(a => a.HoldPlaced).FirstOrDefault();
            CMSLibraryCard card = earliestHold.LibraryCard;
            _context.Remove(earliestHold);
            _context.SaveChanges();

            CheckoutItem(assetId, card.Id);
        }

        /// <summary>
        /// Getting the checkout Histories for the item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistory
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);
        }

        /// <summary>
        /// Getting the latest checkout
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Checkout GetLatestCheckout(int id)
        {
            return _context.Checkout
                .Where(a => a.LibraryAsset.Id == id)
                .Include(a => a.LibraryCard)
                .OrderByDescending(a => a.Since)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returning the available copies
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetAvailableCopies(int id)
        {
            int numberOfCopies = GetNumberOfCopies(id);

            int numberCheckedOut = _context.Checkout
                .Where(a => a.LibraryAsset.Id == id
                         && a.LibraryAsset.Status.Name == "Checked Out")
                         .Count();

            return numberOfCopies - numberCheckedOut;
        }

        /// <summary>
        /// Getting the number of copies
        /// TODO: Implement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetNumberOfCopies(int id)
        {
            return _context.CMSLibraryAsset
                .FirstOrDefault(a => a.Id == id)
                .NumberOfCopies;
        }

        /// <summary>
        /// Setting the default checkout time
        /// TODO: should be per item thus in the DB
        /// </summary>
        /// <param name="now"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        private DateTime GetDefaultCheckoutTime(DateTime now, int assetId)
        {
            switch (GetType(assetId))
            {
                case "Book":
                    return now.AddDays(30);
                case "Video":
                case "Magazine":
                    return now.AddDays(2);
                case "NewsPaper":
                    return now.AddDays(1);
                default:
                    return now;
            }
        }

        /// <summary>
        /// TODO: Not implemented
        /// </summary>
        /// <param name="holdId"></param>
        /// <returns></returns>
        public string GetCurrentHoldSubscriberName(int holdId)
        {
            var hold = _context.Hold
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(v => v.Id == holdId);

            int cardId = hold
                .Include(a => a.LibraryCard)
                .Select(a => a.LibraryCard.Id)
                .FirstOrDefault();

            Subscriber subscriber = _context.Subscribers
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            // TODO: Do this with a db computed value
            return subscriber?.FirstName + " " + subscriber?.LastName;
        }

        /// <summary>
        /// Returns the time of the Hold in question
        /// </summary>
        /// <param name="holdId"></param>
        /// <returns></returns>
        public DateTime GetCurrentHoldPlacedTime(int holdId)
        {
            return _context.Hold
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.Id == holdId)
                .HoldPlaced;
        }

        /// <summary>
        /// Returns all holds for the item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Hold
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);
        }

        /// <summary>
        /// Gets the name of the subscriber who has checked out the item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCurrentCheckoutSubscriberName(int id)
        {
            Checkout checkout = GetCheckoutByAssetId(id);
            if (checkout == null)
            {
                return "";
            }

            Subscriber subscriber = _context.Subscribers
                .Include(p => p.LibraryCard)
                .Where(c => c.LibraryCard.Id == checkout.LibraryCard.Id)
                .FirstOrDefault();

            // TODO: Do this with a db computed value
            return subscriber?.FirstName + " " + subscriber?.LastName;
        }

        /// <summary>
        /// Returns the checkout placed for the asset
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public Checkout GetCheckoutByAssetId(int assetId)
        {
            return _context.Checkout
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == assetId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Removes all the checkout history
        /// </summary>
        /// <param name="assetId"></param>
        public void RemoveChekoutHistory(int assetId)
        {
            _context.RemoveRange(GetCheckoutHistory(assetId));
            _context.SaveChanges();
        }
    }
}
