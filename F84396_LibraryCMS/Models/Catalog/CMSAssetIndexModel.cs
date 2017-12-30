using System.Collections.Generic;

namespace F84396_LibraryCMS.Models.Catalog
{
    /// <summary>
    /// Views/Catalog/Index
    /// </summary>
    public class CMSAssetIndexModel
    {
        public IEnumerable<CMSAssetsListing> Assets { get; set; }
    }
}
