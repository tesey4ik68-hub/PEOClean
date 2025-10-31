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
        public bool IsConfirmed { get; set; } = false;

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public string? PhotoPath { get; set; } // Path to photo file

        // Добавленные свойства
        public int? AttendanceRecordId { get; set; }

        // Navigation properties
        public virtual Employee Employee { get; set; } = null!;
        public virtual ServiceAddress ServiceAddress { get; set; } = null!;
        public virtual WorkType WorkType { get; set; } = null!;
        
        // Добавленная навигационная ссылка
        public virtual AttendanceRecord? AttendanceRecord { get; set; }

        // Вспомогательное свойство для получения списка фото из строки PhotoPath
        public ICollection<string> PhotoFiles => 
            string.IsNullOrEmpty(PhotoPath) ? new List<string>() : PhotoPath.Split(';').ToList();
    }
}