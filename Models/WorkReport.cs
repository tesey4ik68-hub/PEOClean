using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public class WorkReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int ServiceAddressId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int WorkTypeId { get; set; }

        public bool IsCompleted { get; set; } = false;

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public string? PhotoPath { get; set; } // Path to photo file

        // Navigation properties
        public virtual Employee Employee { get; set; } = null!;
        public virtual ServiceAddress ServiceAddress { get; set; } = null!;
        public virtual WorkType WorkType { get; set; } = null!;
    }
}
