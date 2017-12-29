using CMSLibraryData;
using CMSLibraryData.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMSLibraryServices
{
    /// <summary>
    /// AssetsBasse: utility class implementing methods to be used across services
    /// </summary>
    public class AssetsBase : IAssetsBase
    {
        private CMSLibraryContext _context;

        public AssetsBase(CMSLibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the type of the asset aka":
        /// Book, Magazine, Newspaper, Video
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetType(int id)
        {
            DbSet<CMSLibraryAsset> assets = _context.CMSLibraryAsset;
            if (assets.OfType<Book>().Where(asset => asset.Id == id).Any())
            {
                return "Book";
            }
            else if (assets.OfType<Video>().Where(asset => asset.Id == id).Any())
            {
                return "Video";
            }
            else if (assets.OfType<Magazine>().Where(asset => asset.Id == id).Any())
            {
                return "Magazine";
            }
            else if (assets.OfType<NewsPaper>().Where(asset => asset.Id == id).Any())
            {
                return "Newspaper";
            }
            return "Unknown";
        }

        /// <summary>
        /// Returns the Status Flags for the asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAssetStatus(int id)
        {
            return _context.CMSLibraryAsset.Include(a => a.Status).FirstOrDefault(a => a.Id == id).Status.Name;
        }

        /// <summary>
        /// Checks to see if the status of the entry is "Checked Out"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsCheckedOut(int id)
        {
            return _context.CMSLibraryAsset.Include(a => a.Status).FirstOrDefault(a => a.Id == id).Status.Name == "Checked Out";
        }
    }
}
