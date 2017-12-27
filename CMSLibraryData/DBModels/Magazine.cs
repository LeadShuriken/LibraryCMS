﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    public class Magazine : CMSLibraryAsset
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }
    }
}
