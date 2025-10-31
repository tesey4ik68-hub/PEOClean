using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Notes { get; set; }

        public bool IsJanitor { get; set; } = false; // Дворник

        public bool IsCleaner { get; set; } = false; // Уборщик(ца)

        public bool IsActive { get; set; } = true; // Активен

        // Computed property for display
        public string EmployeeType
        {
            get
            {
                if (IsJanitor && IsCleaner)
                    return "Дворник/Уборщик";
                else if (IsJanitor)
                    return "Дворник";
                else if (IsCleaner)
                    return "Уборщик";
                else
                    return "";
            }
        }

        // Navigation properties
        public virtual ICollection<EmployeeServiceAddress> EmployeeServiceAddresses { get; set; } = new List<EmployeeServiceAddress>();
        public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public virtual ICollection<WorkReport> WorkReports { get; set; } = new List<WorkReport>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
