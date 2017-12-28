using CMSLibraryData;
using CMSLibraryData.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMSLibraryServices
{
    public class AssetsBase : IAssetsBase
    {
        private CMSLibraryContext _context;

        public AssetsBase(CMSLibraryContext context)
        {
            _context = context;
        }

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
    }
}
