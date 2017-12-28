using System;
using System.Collections.Generic;
using CMSLibraryData;
using CMSLibraryData.DBModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMSLibraryServices
{
    public class CheckoutService : AssetsBase , ICheckout
    {
        private CMSLibraryContext _context;

        public CheckoutService(CMSLibraryContext context)
             : base(context)
        {
            _context = context;
        }

        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public Checkout Get(int id)
        {
            return _context.Checkout.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkout;
        }

        public void CheckoutItem(int assetId, int libraryCardId)
        {
            if (IsCheckedOut(assetId))
            {
                return;
            }

            DateTime now = DateTime.Now;
            CMSLibraryAsset item = _context.CMSLibraryAsset
                .Include(a => a.Status)
                .FirstOrDefault(a => a.Id == assetId);

            UpdateStatus(assetId, "Checked Out");

            CMSLibraryCard libraryCard = _context.CMSLibraryCard
                .Include(c => c.Checkouts)
                .FirstOrDefault(a => a.Id == libraryCardId);

            if (libraryCard == null)
            {
                return;
            }

            Checkout checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now, assetId)
            };

            _context.Add(checkout);
            var b = _context.GetType();

            CheckoutHistory checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        public void MarkLost(int assetId)
        {
            UpdateStatus(assetId, "Lost");
            _context.SaveChanges();
        }

        public void MarkFound(int assetId)
        {
            DateTime now = DateTime.Now;

            UpdateStatus(assetId, "Available");
            RemoveExistingCheckout(assetId);
            CloseExistingCheckoutHistory(assetId, now);

            _context.SaveChanges();
        }

        private void UpdateStatus(int assetId, string v)
        {
            CMSLibraryAsset item = _context.CMSLibraryAsset
                .FirstOrDefault(a => a.Id == assetId);
            _context.Update(item);
            item.Status = _context.Status.FirstOrDefault(a => a.Name == v);
        }

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

        public void PlaceHold(int assetId, int libraryCardId)
        {
            DateTime now = DateTime.Now;

            CMSLibraryAsset asset = _context.CMSLibraryAsset
                .Include(a => a.Status)
                .FirstOrDefault(a => a.Id == assetId);

            CMSLibraryCard libraryCard = _context.CMSLibraryCard
                .FirstOrDefault(a => a.Id == libraryCardId);

            IEnumerable<Hold> currentHolds = GetCurrentHolds(assetId);
            Checkout currentCheckout = GetLatestCheckout(assetId);
            if (libraryCard == null // invalid card
                || currentCheckout.LibraryCard.Id == libraryCardId // checkouter wants to hold
                || currentHolds.Any(a => a.LibraryCard.Id == libraryCardId)) // second hold in queue
            {
                return;
            }

            _context.Update(asset);

            if (asset.Status.Name == "Available")
            {
                UpdateStatus(assetId, "On Hold");
            }

            Hold hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = libraryCard
            };

            _context.Add(hold);
            _context.SaveChanges();
        }

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

        private void CheckoutToEarliestHold(int assetId, IEnumerable<Hold> currentHolds)
        {
            Hold earliestHold = currentHolds.OrderBy(a => a.HoldPlaced).FirstOrDefault();
            CMSLibraryCard card = earliestHold.LibraryCard;
            _context.Remove(earliestHold);
            _context.SaveChanges();

            CheckoutItem(assetId, card.Id);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistory
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);
        }

        public Checkout GetLatestCheckout(int id)
        {
            return _context.Checkout
                .Where(a => a.LibraryAsset.Id == id)
                .Include(a => a.LibraryCard)
                .OrderByDescending(a => a.Since)
                .FirstOrDefault();
        }

        public int GetAvailableCopies(int id)
        {
            int numberOfCopies = GetNumberOfCopies(id);

            int numberCheckedOut = _context.Checkout
                .Where(a => a.LibraryAsset.Id == id
                         && a.LibraryAsset.Status.Name == "Checked Out")
                         .Count();

            return numberOfCopies - numberCheckedOut;
        }

        public int GetNumberOfCopies(int id)
        {
            return _context.CMSLibraryAsset
                .FirstOrDefault(a => a.Id == id)
                .NumberOfCopies;
        }

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

        public bool IsCheckedOut(int id)
        {
            return _context.Checkout.Where(a => a.LibraryAsset.Id == id).Any();
        }

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

        public DateTime GetCurrentHoldPlaced(int holdId)
        {
            return _context.Hold
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.Id == holdId)
                .HoldPlaced;
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Hold
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);
        }

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

        public Checkout GetCheckoutByAssetId(int assetId)
        {
            return _context.Checkout
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == assetId)
                .FirstOrDefault();
        }

        public void RemoveChekoutHistory(int assetId)
        {
            _context.RemoveRange(GetCheckoutHistory(assetId));
            _context.SaveChanges();
        }
    }
}
