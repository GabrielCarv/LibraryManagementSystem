using Microsoft.EntityFrameworkCore;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Library_Management_System.Data;

namespace Library_Management_System.Controllers
{
    public class CategoriesController : MainController
    {
        public CategoriesController(LibraryContext context) : base(context){}

        public async Task<IActionResult> Index()
        {
            ViewBag.classId = 3;
            return View(await _context.Categories.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (IsIdNull(id))   return NotFound();

            Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (!IsContextValued(category)) return NotFound();
            ViewBag.classId = 3;
            return View(category);
        }

        public IActionResult Create()
        {
            ViewBag.classId = 3;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            if (DoesItAlreadyExist(category.Id, category.Name))
                ModelState.AddModelError("", "Category Name Already Exist!");

            return await DataBaseTransacion("insert", category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (IsIdNull(id)) return NotFound();

            Category? category = await _context.Categories.FindAsync(id);

            if (!IsContextValued(category)) return NotFound();
            ViewBag.classId = 3;
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name")] Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            if (id != category.Id)
                return BadRequest();

            if (DoesItAlreadyExist(category.Id, category.Name))
                ModelState.AddModelError("", "Category already does exist!");

            return await DataBaseTransacion("update", category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (IsIdNull(id)) return NotFound();

            Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (!IsContextValued(category)) return NotFound();
            ViewBag.classId = 3;
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);
            return await DataBaseTransacion("delete", category);
        }

        private bool IsContextValued(Category? category)
        {
            return category != null;
        }

        private bool DoesItAlreadyExist(int id, string name)
        {
            bool anyEquals = _context.Categories.AsNoTracking().Any(e => e.Id == id) || _context.Categories.AsNoTracking().Any(e => e.Name == name);
            return anyEquals;
        }

        private async Task<IActionResult> DataBaseTransacion(string databaseCommand, Category category)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                transaction.CreateSavepoint("Before");

                switch (databaseCommand.ToLower())
                {
                    case "insert":
                        _context.Categories.Add(category);
                        break;
                    case "update":
                        _context.Categories.Update(category);
                        break;
                    case "delete":
                        _context.Categories.Remove(category);
                        break;
                    default:
                        throw new Exception("Please name a correct database command");
                }
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                transaction.RollbackToSavepoint("Before");
                return BadRequest();
            }
            finally
            {
                _context.Dispose();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
