using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
