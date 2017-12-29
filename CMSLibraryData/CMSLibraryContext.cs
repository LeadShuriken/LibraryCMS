using Microsoft.EntityFrameworkCore;
using CMSLibraryData.DBModels;

namespace CMSLibraryData
{
    /// <summary>
    /// Context for the SQL database
    /// </summary>
    public class CMSLibraryContext : DbContext
    {
        public CMSLibraryContext(DbContextOptions options) : base(options) { }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Checkout> Checkout { get; set; }
        public DbSet<Hold> Hold { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistory { get; set; }
        public DbSet<CMSLibraryCard> CMSLibraryCard { get; set; }
        public DbSet<CMSLibraryAsset> CMSLibraryAsset { get; set; }
        public DbSet<CMSLibraryBranch> CMSLibraryBranch { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BranchHours> BranchHours { get; set; }
        public DbSet<Magazine> Magazine { get; set; }
        public DbSet<NewsPaper> NewsPaper { get; set; }
        public DbSet<Video> Video { get; set; }
    }
}