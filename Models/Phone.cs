using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Phone
    {
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Key]
        [Required(AllowEmptyStrings = false)]
        public string PhoneNumber { get; set; }

        public string IdPerson { get; set; }
        public virtual Person? Person { get; set; }

        public Phone(string phoneNumber, string idPerson)
        {
            PhoneNumber = phoneNumber;
            IdPerson = idPerson;
        }
    }
}
