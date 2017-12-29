using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity CMSLibraryBranch 
    /// Spesifying the location of a CMSLibraryAsset and
    /// relating to the address of the Subscriber (HomeLibraryBranchId)
    /// </summary>
    public class CMSLibraryBranch
    {
        public int Id { get; set; }

        [Required, Display(Name = "Branch Name")]
        [StringLength(30, ErrorMessage = "Limit branch name to 30 characters.")]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Telephone { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }

        public virtual IEnumerable<Subscriber> Subscriber { get; set; }
        public virtual IEnumerable<CMSLibraryAsset> LibraryAssets { get; set; }

        public string ImageUrl { get; set; }
    }
}
