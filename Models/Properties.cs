using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Properties
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Damaged")]
        public bool IsDamaged { get; set; }
        public string Info { get; set; }
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }
    }
}

