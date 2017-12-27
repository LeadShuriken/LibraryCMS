using Microsoft.EntityFrameworkCore;
using CMSLibraryData.DBModels;

namespace CMSLibraryData
{
    public class CMSLibraryContext : DbContext
    {
        public CMSLibraryContext(DbContextOptions options) : base(options) { }

        public DbSet<Subscriber> Subscribers { get; set; }
    }
}
