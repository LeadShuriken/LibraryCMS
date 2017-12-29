using System;
using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity CheckoutHistory
    /// Spesifies the relation between a CMSLibraryAsset, CMSLibraryCard 
    /// when an element passes through a checkout, this is persistant unlike Checkout
    /// </summary>
    public class CheckoutHistory
    {
        public int Id { get; set; }

        [Required]
        public CMSLibraryAsset LibraryAsset { get; set; }

        [Required]
        public CMSLibraryCard LibraryCard { get; set; }

        [Required]
        public DateTime CheckedOut { get; set; }

        public DateTime? CheckedIn { get; set; }
    }
}
