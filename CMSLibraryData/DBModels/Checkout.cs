using System;
using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    public class Checkout
    {
        public int Id { get; set; }

        [Required, Display(Name = "Library Asset")]
        public CMSLibraryAsset LibraryAsset { get; set; }

        [Display(Name = "Library Card")]
        public CMSLibraryCard LibraryCard { get; set; }

        [Display(Name = "Checked Out Since")]
        public DateTime Since { get; set; }

        [Display(Name = "Checked Out Until")]
        public DateTime Until { get; set; }
    }
}
