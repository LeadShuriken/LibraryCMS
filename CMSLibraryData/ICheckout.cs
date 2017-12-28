using System.Collections.Generic;
using CMSLibraryData.DBModels;
using System;

namespace CMSLibraryData
{
    public interface ICheckout : IAssetsBase
    {
        void Add(Checkout newCheckout);

        IEnumerable<Checkout> GetAll();
        IEnumerable<Hold> GetCurrentHolds(int id);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);

        Checkout Get(int id);
        Checkout GetLatestCheckout(int id);

        void MarkLost(int id);
        void MarkFound(int id);
        void PlaceHold(int assetId, int libraryCardId);
        void CheckoutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId);
        void RemoveChekoutHistory(int assetId);

        int GetNumberOfCopies(int id);
        int GetAvailableCopies(int id);
        bool IsCheckedOut(int id);

        DateTime GetCurrentHoldPlaced(int id);
        string GetCurrentCheckoutSubscriberName(int id);
        string GetCurrentHoldSubscriberName(int id);
    }
}
