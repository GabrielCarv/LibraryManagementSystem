using System.ComponentModel.DataAnnotations;
using Library_Management_System.Entity;

namespace Library_Management_System.Models
{
    public class Publisher : EntityBase
    {
        [Required]
        public string Name { get; set; }

    }
}
