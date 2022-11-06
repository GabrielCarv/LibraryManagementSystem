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
            try
            {
                List<Category> categories = await _context.Categories.AsNoTracking().ToListAsync();
                ViewBag.classId = 3;
                return View(categories);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (IsIdNull(id)) return NotFound();

                Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

                if (!IsContextValued(category)) return NotFound();
                ViewBag.classId = 3;
                return View(category);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            
        }

        public IActionResult Create()
        {
            try
            {
                ViewBag.classId = 3;
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(category);

                if (DoesItAlreadyExist(category.Id, category.Name))
                {
                    ModelState.AddModelError("", "Category Name Already Exist!");
                    return View(category);
                }

                return await DataBaseTransacion("insert", category);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (IsIdNull(id)) return NotFound();

                Category? category = await _context.Categories.FindAsync(id);

                if (!IsContextValued(category)) return NotFound();
                ViewBag.classId = 3;
                return View(category);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(category);

                if (id != category.Id)
                    return BadRequest();

                if (DoesItAlreadyExist(category.Id, category.Name))
                {
                    ModelState.AddModelError("", "Category already does exist!");
                    return View(category);
                }

                return await DataBaseTransacion("update", category);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (IsIdNull(id)) return NotFound();

                Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

                if (!IsContextValued(category)) return NotFound();
                ViewBag.classId = 3;
                return View(category);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Category? category = await _context.Categories.FindAsync(id);
                return await DataBaseTransacion("delete", category);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsContextValued(Category? category)
        {
            return category != null;
        }

        private bool DoesItAlreadyExist(int id, string name)
        {
            //_context.Categories.AsNoTracking().Any(e => e.Id == id) &&
            bool anyNameEquals = _context.Categories.AsNoTracking().Any(e => e.Name == name);
            return anyNameEquals;
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
