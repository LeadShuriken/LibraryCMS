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

        public void Add(Subscriber newSubscriber)
        {
            _context.Add(newSubscriber);
            _context.SaveChanges();
        }

        public Subscriber Get(int id)
        {
            return _context.Subscribers
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Subscriber> GetAll()
        {
            return _context.Subscribers
                .Include(a=>a.LibraryCard)
                .Include(a=>a.HomeLibraryBranch);
        }

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

        public IEnumerable<Checkout> GetCheckouts(int id)
        {
            return _context.Checkout
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(v => v.LibraryCard.Id == Get(id).LibraryCard.Id);
        }

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
