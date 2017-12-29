using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity of the CMSLibraryAsset::NewsPaper
    /// </summary>
    public class NewsPaper : CMSLibraryAsset
    {
        [Required]
        public string Publisher { get; set; }
    }
}
