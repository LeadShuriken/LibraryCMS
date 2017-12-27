using System;
using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
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
