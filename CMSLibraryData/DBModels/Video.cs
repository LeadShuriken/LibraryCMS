using System;
using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    public class Video : CMSLibraryAsset
    {
        [Required]
        public string Director { get; set; }
    }
}
