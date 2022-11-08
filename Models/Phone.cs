using System.ComponentModel.DataAnnotations;
using Library_Management_System.Entity;

namespace Library_Management_System.Models
{
    public class Phone : EntityBase
    {
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Key]
        [Required(AllowEmptyStrings = false)]
        public string PhoneNumber { get; set; }

        public string PersonCpf { get; set; }
        public virtual Person? Person { get; set; }

        public Phone(string phoneNumber, string personCpf)
        {
            PhoneNumber = phoneNumber;
            PersonCpf = personCpf;
        }
    }
}
