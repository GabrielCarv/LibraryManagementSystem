using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Category
    {
        public Category()
        {
            BookCategories = new HashSet<BookCategory>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<BookCategory> BookCategories { get; set; }
    }
}
