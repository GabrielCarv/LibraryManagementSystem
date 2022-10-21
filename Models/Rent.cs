using System.ComponentModel.DataAnnotations;
namespace Library_Management_System.Models
{
    public class Rent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Rental Date")]
        public DateTime RentalDate { get; set; }
        [Required]
        [Display(Name = "Rent Return Date")]
        public DateTime RentReturnDate { get; set; }

        [Display(Name = "Real Rent Return Date")]
        public DateTime RentRealReturnDate { get; set; }

        public virtual Person? Person { get; set; } 
        public virtual Properties? Properties { get; set; } 
    }
}
