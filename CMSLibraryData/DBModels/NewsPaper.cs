using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    public class NewsPaper : CMSLibraryAsset
    {
        [Required]
        public string Publisher { get; set; }
    }
}
