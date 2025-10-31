using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public enum UnitOfMeasure
    {
        Дом,
        Подъезд,
        М2,
        Шт,
        Час
    }

    public class WorkType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // e.g., "Мойка подъездов", "Уборка двора", etc.

        [MaxLength(10)]
        public string Code { get; set; } = string.Empty; // Короткий код для табеля

        public UnitOfMeasure UnitOfMeasure { get; set; } = UnitOfMeasure.Дом; // Единица измерения

        public bool RequiresPhoto { get; set; } = false; // Требует фото?

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<HouseWorkSchedule> HouseWorkSchedules { get; set; } = new List<HouseWorkSchedule>();
        public virtual ICollection<WorkReport> WorkReports { get; set; } = new List<WorkReport>();
    }
}
