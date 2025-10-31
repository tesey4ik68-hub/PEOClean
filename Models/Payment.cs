using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime PeriodStart { get; set; }

        [Required]
        public DateTime PeriodEnd { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public bool IsPaid { get; set; } = false;

        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty;

        // Navigation property
        public virtual Employee Employee { get; set; } = null!;
    }
}
