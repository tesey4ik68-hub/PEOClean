using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public class AttendanceRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public AbsenceType Status { get; set; }

        public int? ReplacementEmployeeId { get; set; } // For replacement cases

        public bool IsConfirmed { get; set; } = false;

        public bool HasPhoto { get; set; } = false;

        // Добавленное свойство для проверки отсутствия
        public bool IsAbsent => Status != AbsenceType.Present;

        // Navigation properties
        public virtual Employee Employee { get; set; } = null!;
        public virtual Employee? ReplacementEmployee { get; set; }

        // Many-to-many with ServiceAddress
        public virtual ICollection<AttendanceRecordServiceAddress> AttendanceRecordServiceAddresses { get; set; } = new List<AttendanceRecordServiceAddress>();
    }
}