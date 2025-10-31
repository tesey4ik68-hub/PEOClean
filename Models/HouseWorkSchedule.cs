using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public class HouseWorkSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ServiceAddressId { get; set; }

        [Required]
        public int WorkTypeId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; } // Monday, Tuesday, etc.

        // Navigation properties
        public virtual ServiceAddress ServiceAddress { get; set; } = null!;
        public virtual WorkType WorkType { get; set; } = null!;
    }
}
