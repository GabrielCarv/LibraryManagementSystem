using System.ComponentModel.DataAnnotations;
using Library_Management_System.Entity;

namespace Library_Management_System.Models
{
    public class Person : EntityBase
    {
        [Key]
        [StringLength(11, MinimumLength = 11)]
        public string Cpf{ get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Email isn't valid.")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Employer")]
        public bool IsEmployer { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
       
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Postal Code")]
        public string PostalCode{ get; set; }
       
        [Required]
        public string State { get; set; }
        [Required]
        public string City  { get; set; }
        [Required]
        public string Address { get; set; }
       
        [Display(Name = "House Number")]
        public int HouseNumber { get; set; }

    }
}

