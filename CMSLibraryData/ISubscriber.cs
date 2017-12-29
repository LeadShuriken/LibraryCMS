using CMSLibraryData.DBModels;
using System.Collections.Generic;

namespace CMSLibraryData
{
    /// <summary>
    /// ISubscriber: spesifying methods, props used by the subscriber
    /// </summary>
    public interface ISubscriber
    {
        Subscriber GetSubscriber(int Id);

        IEnumerable<Subscriber> GetAllSubscribers();
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int Id);
        IEnumerable<Hold> GetHolds(int Id);
        IEnumerable<Checkout> GetCheckouts(int Id);
    }
}
