using System.ComponentModel.DataAnnotations;

namespace CMSLibraryData.DBModels
{
    public class Status
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
