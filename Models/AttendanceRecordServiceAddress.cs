using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public class AttendanceRecordServiceAddress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AttendanceRecordId { get; set; }

        [Required]
        public int ServiceAddressId { get; set; }

        // Navigation properties
        public virtual AttendanceRecord AttendanceRecord { get; set; } = null!;
        public virtual ServiceAddress ServiceAddress { get; set; } = null!;
    }
}
