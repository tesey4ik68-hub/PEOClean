using System.ComponentModel.DataAnnotations;

namespace PEOcleanWPFApp.Models
{
    public enum EmployeeRole
    {
        [Display(Name = "Дворник")]
        Janitor,
        
        [Display(Name = "Уборщик")]
        Cleaner
    }
}