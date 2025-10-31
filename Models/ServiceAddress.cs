using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public class ServiceAddress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        public int Floors { get; set; }

        public int Entrances { get; set; }

        public int Apartments { get; set; } // Жилых помещений

        public decimal HouseArea { get; set; } // Площадь дома

        public decimal? YardArea { get; set; } // Площадь двора

        [Required]
        public decimal MonthlyRateJanitor { get; set; } // Месячная ставка дворника

        [Required]
        public decimal MonthlyRateCleaner { get; set; } // Месячная ставка уборщицы

        [MaxLength(50)]
        public string ObjectType { get; set; } = "Многоквартирный дом"; // Тип объекта

        [MaxLength(50)]
        public string GarbageChuteType { get; set; } = "отсутствует"; // Тип мусоропровода

        [MaxLength(50)]
        public string BuildingType { get; set; } = "панельный"; // Тип постройки

        public int ConstructionYear { get; set; } // Год постройки

        // Navigation properties
        public virtual ICollection<EmployeeServiceAddress> EmployeeServiceAddresses { get; set; } = new List<EmployeeServiceAddress>();
        public virtual ICollection<HouseWorkSchedule> HouseWorkSchedules { get; set; } = new List<HouseWorkSchedule>();
        public virtual ICollection<AttendanceRecordServiceAddress> AttendanceRecordServiceAddresses { get; set; } = new List<AttendanceRecordServiceAddress>();
        public virtual ICollection<WorkReport> WorkReports { get; set; } = new List<WorkReport>();
    }
}
