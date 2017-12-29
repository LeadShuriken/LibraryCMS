using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity CMSLibraryAsset 
    /// Asset has a discriminator servers the Table per Hierarchy (TPH) migration strategy
    /// this property makes the assets aider a Magazine, Book, Video, Newspaper.
    /// The asset is situated in a spesific library branch and has a spesific 
    /// Status, CheckoutHistories and is linked to a Checkout or a Hold if aider.
    /// </summary>
    public abstract class CMSLibraryAsset
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Index")]
        public string Index { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; } // Just store as an int for BC

        [Display(Name = "Publish Date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? PublishDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required, Display(Name = "Cost of Replacement")]
        public decimal Cost { get; set; }

        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }

        public virtual CMSLibraryBranch Location { get; set; }
    }
}
