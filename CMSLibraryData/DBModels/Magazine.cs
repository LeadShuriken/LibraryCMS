using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity of the CMSLibraryAsset::Magazine
    /// </summary>
    public class Magazine : CMSLibraryAsset
    {
        [Required]
        public string Agency { get; set; }
    }
}
