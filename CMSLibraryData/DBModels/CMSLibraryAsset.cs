using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSLibraryData.DBModels
{
    public abstract class CMSLibraryAsset
    {
        public int Id { get; set; }

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
