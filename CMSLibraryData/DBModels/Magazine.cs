using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    public class Magazine : CMSLibraryAsset
    {
        [Required]
        public string Agency { get; set; }
    }
}
