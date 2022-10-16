using Library_Management_System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public abstract class MainController: Controller
    {
        protected readonly LibraryContext _context;

        public MainController(LibraryContext context)
        {
            _context = context;
        }

        protected bool IsIdNull(int? id)
        {
            return !id.HasValue;    
        }
    }
}
