using CMSLibraryData;
using F84396_LibraryCMS.Models.Catalog;
using F84396_LibraryCMS.Models.Checkouts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace F84396_LibraryCMS.Controllers
{
    public class CatalogController : Controller
    {
        private ICMSLibraryAsset _assets;
        private ICheckout _checkouts;

        public CatalogController(ICMSLibraryAsset assets, ICheckout checkouts)
        {
            _assets = assets;
            _checkouts = checkouts;
        }

        public IActionResult Index()
        {
            IEnumerable<CMSLibraryData.DBModels.CMSLibraryAsset> assetModels = _assets.GetAll();
            IEnumerable<CMSAssetsListing> listingResult = assetModels.Select(result => new CMSAssetsListing
            {
                Id = result.Id,
                ImageUrl = result.ImageUrl,
                Index = _assets.GetIndex(result.Id),
                Source = _assets.GetSource(result.Id),
                Title = result.Title,
                Type = _assets.GetType(result.Id)
            });

            CMSAssetIndexModel model = new CMSAssetIndexModel()
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id, string messege)
        {
            CMSLibraryData.DBModels.CMSLibraryAsset asset = _assets.GetById(id);

            CMSAssetDetailModel model = new CMSAssetDetailModel()
            {
                AssetId = id,
                Title = asset.Title,
                Type = _assets.GetType(id),
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                Source = _assets.GetSource(id),
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                Index = _assets.GetIndex(id),
                CheckoutHistory = _checkouts.GetCheckoutHistory(id),
                ISBN = _assets.GetIsbn(id),
                LatestCheckout = _checkouts.GetLatestCheckout(id),
                SubscriberName = _checkouts.GetCurrentCheckoutSubscriberName(id),
                CurrentHolds = _checkouts.GetCurrentHolds(id).Select(a => new AssetHoldModel
                {
                    HoldPlaced = _checkouts.GetCurrentHoldPlaced(a.Id),
                    SubscriberName = _checkouts.GetCurrentHoldSubscriberName(a.Id)
                })
            };

            if (messege != null) {
                model.Warning = new AssetWarning { Messege = messege };
            }

            return View(model);
        }

        // These are almost the same
        public IActionResult Checkout(int id)
        {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id)
            };
            return View(model);
        }

        public IActionResult Hold(int id)
        {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                HoldCount = _checkouts.GetCurrentHolds(id).Count()
            };
            return View(model);
        }
        // These are almost the same

        public IActionResult CheckIn(int id)
        {
            _checkouts.CheckInItem(id);
            return RedirectToAction("Detail", new { id = id });
        }

        public IActionResult MarkLost(int id)
        {
            _checkouts.MarkLost(id);
            _checkouts.RemoveChekoutHistory(id);
            return RedirectToAction("Detail", new { id = id });
        }

        public IActionResult MarkFound(int id)
        {
            _checkouts.MarkFound(id);
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId)
        {
            _checkouts.CheckoutItem(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            string info = _checkouts.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId , messege = info});
        }
    }
}