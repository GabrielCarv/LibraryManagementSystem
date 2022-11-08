using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library_Management_System.Entity;

namespace Library_Management_System.Models
{
    public class Book : EntityBase
    {
        public string Title { get; set; }
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
