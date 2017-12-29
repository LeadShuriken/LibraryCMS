using CMSLibraryData;
using CMSLibraryData.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CMSLibraryServices
{
    /// <summary>
    /// Asset managment services
    /// </summary>
    public class CMSLibraryAssetService : AssetsBase, ICMSLibraryAsset
    {
        private CMSLibraryContext _context;

        public CMSLibraryAssetService(CMSLibraryContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all asset in the system
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CMSLibraryAsset> GetAll()
        {
            return _context.CMSLibraryAsset
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }

        /// <summary>
        /// Gets an asset by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CMSLibraryAsset GetById(int id)
        {
            return GetAll().FirstOrDefault(asset => asset.Id == id);
        }

        /// <summary>
        /// Returns the location of an asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CMSLibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
        }

        /// <summary>
        /// Returns the index of an asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetIndex(int id)
        {
            return GetById(id).Index;   
        }

        /// <summary>
        /// Return the ISBN (only if a book)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetIsbn(int id)
        {
            if (_context.Book.Any(book => book.Id == id))
            {
                return _context.Book.FirstOrDefault(book => book.Id == id).ISBN;
            }
            return "";
        }

        /// <summary>
        /// Returns the title of the asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTitle(int id)
        {
            return GetById(id).Title;
        }
        
        /// <summary>
        /// Returns the source according the type of asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSource(int id)
        {   
            switch (GetType(id))
            {
                case "Book":
                    return _context.Book.FirstOrDefault(asset => asset.Id == id).Author;
                case "Video":
                    return _context.Video.FirstOrDefault(asset => asset.Id == id).Director;
                case "Magazine":
                    return _context.Magazine.FirstOrDefault(asset => asset.Id == id).Agency;
                case "NewsPaper":
                    return _context.NewsPaper.FirstOrDefault(asset => asset.Id == id).Publisher;
                default:
                    return "Unknown";
            }
        }
    }
}
