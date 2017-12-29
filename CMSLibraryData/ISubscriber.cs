using CMSLibraryData.DBModels;
using System.Collections.Generic;

namespace CMSLibraryData
{
    public interface ISubscriber
    {
        Subscriber Get(int Id);
        void Add(Subscriber newBook);

        IEnumerable<Subscriber> GetAll();
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int Id);
        IEnumerable<Hold> GetHolds(int Id);
        IEnumerable<Checkout> GetCheckouts(int Id);
    }
}
