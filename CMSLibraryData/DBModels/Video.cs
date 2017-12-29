using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity of the CMSLibraryAsset::Video
    /// </summary>
    public class Video : CMSLibraryAsset
    {
        [Required]
        public string Director { get; set; }
    }
}
