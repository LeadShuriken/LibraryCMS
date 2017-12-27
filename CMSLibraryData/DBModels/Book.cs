using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    public class Book : CMSLibraryAsset
    {
        [Required]
        [Display(Name ="ISBN #")]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Index")]
        public string Index { get; set; }
    }
}
