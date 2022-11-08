using System.ComponentModel.DataAnnotations;
using Library_Management_System.Entity;

namespace Library_Management_System.Models
{
    public class Category : EntityBase
    {
        public Category() => BookCategories = new HashSet<BookCategory>();

        public string Name { get; set; }
        public virtual ICollection<BookCategory> BookCategories { get; set; }
    }
}
