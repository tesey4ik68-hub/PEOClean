using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEOcleanWPFApp.Models
{
    public class EmployeeAssignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int ServiceAddressId { get; set; }

        [Required]
        public EmployeeRole Role { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime? EndDate { get; set; }

        public bool IsPrimary { get; set; } = false;

        // Навигационные свойства
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;

        [ForeignKey("ServiceAddressId")]
        public virtual ServiceAddress ServiceAddress { get; set; } = null!;
        
        // Вычисляемое свойство для получения ставки в зависимости от роли
        [NotMapped]
        public decimal Rate => Role == EmployeeRole.Janitor 
            ? ServiceAddress.JanitorRate 
            : ServiceAddress.CleanerRate;
            
        // Вычисляемое свойство для отображения роли
        [NotMapped]
        public string RoleDisplay => Role == EmployeeRole.Janitor ? "Дворник" : "Уборщик";
    }
}