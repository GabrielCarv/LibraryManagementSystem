using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library_Management_System.Models.ViewModel
{
    public class BookCategoryViewModel
    {
        public Book Book { get; set; }

        public List<Category> Categories { get; set; }

        public IEnumerable<SelectListItem> ListItemCategory { get; set; }

        public IEnumerable<int> SelectedCategories { get; set; }
    }
}
