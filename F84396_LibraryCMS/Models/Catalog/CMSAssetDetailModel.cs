using System;
using CMSLibraryData.DBModels;
using System.Collections.Generic;

namespace F84396_LibraryCMS.Models.Catalog
{
    /// <summary>
    /// Views/Catalog/Detail
    /// </summary>
    public class CMSAssetDetailModel
    {
        public int AssetId { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Index { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
        public string CurrentLocation { get; set; }
        public string ImageUrl { get; set; }
        public string SubscriberName { get; set; }
        public Checkout LatestCheckout { get; set; }
        public AssetWarning Warning { get; set; }

        public IEnumerable<CheckoutHistory> CheckoutHistory { get; set; }
        public IEnumerable<AssetHoldModel> CurrentHolds { get; set; }
    }

    public class AssetHoldModel
    {
        public string SubscriberName { get; set; }
        public DateTime HoldPlaced { get; set; }
    }

    public class AssetWarning
    {
        public string Messege { get; set; }
    }
}