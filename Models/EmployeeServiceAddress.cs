using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public class EmployeeServiceAddress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int ServiceAddressId { get; set; }

        public DateTime? StartDate { get; set; } // Optional start date for assignment

        public DateTime? EndDate { get; set; } // Optional end date for assignment

        // Navigation properties
        public virtual Employee Employee { get; set; } = null!;
        public virtual ServiceAddress ServiceAddress { get; set; } = null!;
    }
}
