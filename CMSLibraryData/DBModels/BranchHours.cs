﻿using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity BranchHours
    /// Spesifies a branch hours for the CMSLibraryBranch
    /// TODO: Not implemented
    /// </summary>
    public class BranchHours
    {
        public int Id { get; set; }
        public CMSLibraryBranch Branch { get; set; }

        [Range(0, 6, ErrorMessage = "Day of Week must be between 0 and 6")]
        public int DayOfWeek { get; set; }

        [Range(0, 23, ErrorMessage = "Hour open must be between 0 and 23")]
        public int OpenTime { get; set; }

        [Range(0, 23, ErrorMessage = "Hour closed must be between 0 and 23")]
        public int CloseTime { get; set; }
    }
}
