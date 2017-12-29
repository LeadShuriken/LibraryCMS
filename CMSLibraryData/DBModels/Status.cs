using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    /// <summary>
    /// DB Entity Status
    /// Variants for: ( Name, Description )
    /// 'Checked Out', 'A library asset that has been checked out'
    /// 'Available', 'A library asset that is available for loan'
    /// 'Lost', 'A library asset that has been lost'
    /// 'On Hold', 'A library asset that has been placed On Hold for loan'
    /// </summary>
    public class Status
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
