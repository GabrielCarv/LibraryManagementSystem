using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_System.Models
{
    public class Book
    {
        public Book()
        {
            BookCategories = new HashSet<BookCategory>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Price")]//"{0:0.00}"
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.###}")]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }
        
        public virtual Publisher? Publisher { get; set; }

        public virtual ICollection<BookCategory> BookCategories { get; set; }
    }
}
