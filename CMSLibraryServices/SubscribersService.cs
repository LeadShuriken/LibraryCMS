using CMSLibraryData;
using System.Collections.Generic;
using CMSLibraryData.DBModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMSLibraryServices
{
    public class SubscribersService : ISubscriber
    {
        private CMSLibraryContext _context;

        public SubscribersService(CMSLibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a subscriber by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Subscriber GetSubscriber(int id)
        {
            return _context.Subscribers
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch)
                .FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Returns all subscribers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subscriber> GetAllSubscribers()
        {
            return _context.Subscribers
                .Include(a=>a.LibraryCard)
                .Include(a=>a.HomeLibraryBranch);
        }

        /// <summary>
        /// Returns the checkout history for a subscriber by looking up his card id
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int subscriberId)
        {
            int cardId = _context.Subscribers
                .Include(a=>a.LibraryCard)
                .FirstOrDefault(a => a.Id == subscriberId)
                .LibraryCard.Id;

            return _context.CheckoutHistory
                .Include(a=>a.LibraryCard)
                .Include(a=>a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a=>a.CheckedOut);
        }

        /// <summary>
        /// Returns the checkouts that the subscriber has made
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Checkout> GetCheckouts(int id)
        {
            return _context.Checkout
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(v => v.LibraryCard.Id == GetSubscriber(id).LibraryCard.Id);
        }

        /// <summary>
        /// Returns the holds of the subscriber
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IEnumerable<Hold> GetHolds(int Id)
        {
            int cardId = _context.Subscribers
                .Include(a=>a.LibraryCard)
                .FirstOrDefault(a => a.Id == Id)
                .LibraryCard.Id;

            return _context.Hold
                .Include(a=>a.LibraryCard)
                .Include(a=>a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.HoldPlaced);
        }
    }
}
