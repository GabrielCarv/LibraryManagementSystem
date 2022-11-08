using System.ComponentModel.DataAnnotations;
using Library_Management_System.Entity;

namespace Library_Management_System.Models
{
    public class Properties : EntityBase
    {
        [Required]
        [Display(Name = "Damaged")]
        public bool IsDamaged { get; set; }
        public string? Info { get; set; }

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }
    }
}

