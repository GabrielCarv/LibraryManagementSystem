namespace Library_Management_System.Models.ViewModel
{
    public class RentViewModel
    {
        public Rent Rent { get; set; }  
        public string RentalDate { get; set; }
        public string RentReturnDate { get; set; }
        public string RentRealReturnDate { get; set; }
        public virtual Person Person { get; set; }
        public virtual Properties Property { get; set; }
        public virtual Book Book { get; set; }
    }
}
