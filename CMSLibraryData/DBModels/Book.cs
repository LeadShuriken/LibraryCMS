using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity of the CMSLibraryAsset::Book
    /// </summary>
    public class Book : CMSLibraryAsset
    {
        [Required]
        [Display(Name ="ISBN #")]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }
    }
}
