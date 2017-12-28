using CMSLibraryData;
using CMSLibraryData.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CMSLibraryServices
{
    public class CMSLibraryAssetService : AssetsBase, ICMSLibraryAsset
    {
        private CMSLibraryContext _context;

        public CMSLibraryAssetService(CMSLibraryContext context)
            : base(context)
        {
            _context = context;
        }

        public void Add(CMSLibraryAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public IEnumerable<CMSLibraryAsset> GetAll()
        {
            return _context.CMSLibraryAsset
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }

        public CMSLibraryAsset GetById(int id)
        {
            return GetAll().FirstOrDefault(asset => asset.Id == id);
        }

        public CMSLibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
        }

        public string GetIndex(int id)
        {
            // there is an index on Books only
            if (_context.Book.Any(book => book.Id == id))
            {
                return _context.Book.FirstOrDefault(book => book.Id == id).Index;
            }
            return "";
        }

        public string GetIsbn(int id)
        {
            if (_context.Book.Any(book => book.Id == id))
            {
                return _context.Book.FirstOrDefault(book => book.Id == id).ISBN;
            }
            return "";
        }

        public string GetTitle(int id)
        {
            return GetById(id).Title;
        }
        
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
