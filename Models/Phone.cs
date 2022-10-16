using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Phone
    {
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Key]
        public string PhoneNumber { get; set; }
        public virtual Person Person { get; set; }
    }
}
