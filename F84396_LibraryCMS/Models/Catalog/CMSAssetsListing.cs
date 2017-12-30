using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F84396_LibraryCMS.Models.Catalog
{    
    /// <summary>
    /// Helper to Index and Details
    /// </summary>
    public class CMSAssetsListing
    {
        public int Id { get; set; }
        public string Index { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
    }
}
 