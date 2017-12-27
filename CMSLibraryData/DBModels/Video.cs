using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    public class Video : LibraryAsset
    {
        [Required]
        public string Director { get; set; }
    }
}
