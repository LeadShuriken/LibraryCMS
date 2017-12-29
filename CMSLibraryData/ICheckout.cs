using System.Collections.Generic;
using CMSLibraryData.DBModels;
using System;

namespace CMSLibraryData
{
    /// <summary>
    /// ICheckout: spesifying the service methods to be used on chekout
    /// </summary>
    public interface ICheckout : IAssetsBase
    {
        IEnumerable<Checkout> GetAll();
        IEnumerable<Hold> GetCurrentHolds(int id);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);

        Checkout Get(int id);
        Checkout GetLatestCheckout(int id);

        void MarkLost(int id);
        void MarkFound(int id);

        string CheckoutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId);
        void RemoveChekoutHistory(int assetId);
        void RemoveHolds(int assetId);

        int GetNumberOfCopies(int id);
        int GetAvailableCopies(int id);

        DateTime GetCurrentHoldPlacedTime(int id);
        string PlaceHold(int assetId, int libraryCardId);
        string GetCurrentCheckoutSubscriberName(int id);
        string GetCurrentHoldSubscriberName(int id);
    }
}
