using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMSLibraryData;
using F84396_LibraryCMS.Models.Catalog;

namespace F84396_LibraryCMS.Controllers
{
    public class CatalogController : Controller
    {
        private ICMSLibraryAsset _assets;
        public CatalogController(ICMSLibraryAsset assets)
        {
            _assets = assets;
        }

        public IActionResult Index()
        {
            IEnumerable<CMSLibraryData.DBModels.CMSLibraryAsset> assetModels = _assets.GetAll();
            var listingResult = assetModels.Select(result => new CMSAssetsListing
            {
                Id = result.Id,
                ImageUrl = result.ImageUrl,
                Index = _assets.GetIndex(result.Id),
                AuthorOrDirectorOrPublisher = _assets.GetAuthorOrDirectorOrPublisher(result.Id),
                Title = result.Title,
                Type = _assets.GetType(result.Id)
            });

            var model = new CMSAssetIndexModel()
            {
                Assets = listingResult
            };

            return View(model);
        }
    }
}