using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library_Management_System.Entity;

namespace Library_Management_System.Models
{
    public class Rent : EntityBase
    {
        [Required]
        [Display(Name = "Rental Date")]
        [Column(TypeName = "Date")]
        public DateTime RentalDate { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        [Display(Name = "Rent Return Date")]
        public DateTime RentReturnDate { get; set; }

        [Display(Name = "Real Rent Return Date")]
        [Column(TypeName = "Date")]
        public DateTime RentRealReturnDate { get; set; }
        public string PersonId { get; set; }
        public int PropertiesId { get; set; }
        public virtual Person? Person { get; set; }
        public virtual Properties? Property { get; set; }

    }
}
