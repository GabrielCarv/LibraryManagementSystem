using Library_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library_Management_System.Controllers
{
    public class PropertiesController : MainController
    {
        public PropertiesController(LibraryContext context) : base(context){}

        public async Task<IActionResult> Index()
        {
            ViewBag.classId = 5;
            return View(await _context.Properties.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (IsIdNull(id))
                return NotFound();

            Properties? properties = await _context.Properties.AsNoTracking().FirstOrDefaultAsync(pc => pc.Id == id);

            if (!IsContextValued(properties))
                return NotFound();
            ViewBag.classId = 5;
            return View(properties);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            ViewBag.classId = 5;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Properties properties)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
                return View(properties);
            }

            if (DoesItAlreadyExist(properties.Id, properties.BookId))
            {
                ModelState.AddModelError("", "Property already does Exist!");
                return View(properties);
            }

            return await DataBaseTransacion("insert", properties);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            if (IsIdNull(id)) return NotFound();

            Properties? properties = await _context.Properties.FindAsync(id);

            if (!IsContextValued(properties)) return NotFound();
            ViewBag.classId = 5;
            return View(properties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Note,IsDamaged")] Properties properties)
        {
            if (!ModelState.IsValid)
                return View(properties);

            if (id != properties.Id)
                return BadRequest();

            if (DoesItAlreadyExist(properties.Id, properties.BookId))
            {
                ModelState.AddModelError("", "Property already does add!");
                return View(properties);
            }

            return await DataBaseTransacion("update", properties);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (IsIdNull(id)) return NotFound();

           Properties? properties = await _context.Properties.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (!IsContextValued(properties)) return NotFound();
            ViewBag.classId = 5;
            return View(properties);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Properties? properties = await _context.Properties.FindAsync(id);

            return await DataBaseTransacion("delete", properties);
        }

        private bool IsContextValued(Properties? properties)
        {
            return properties != null;
        }

        private bool DoesItAlreadyExist(int id, int bookId)
        {
            bool itsAtDataBase = _context.Properties.AsNoTracking().Any(e => e.Id == id) || _context.Properties.AsNoTracking().Any(e => e.BookId == bookId);
            return itsAtDataBase;
        }


        private async Task<IActionResult> DataBaseTransacion(string databaseCommand, Properties properties)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                transaction.CreateSavepoint("Before");

                switch (databaseCommand.ToLower())
                {
                    case "insert":
                        _context.Properties.Add(properties);
                        break;
                    case "update":
                        _context.Properties.Update(properties);
                        break;
                    case "delete":
                        _context.Properties.Remove(properties);
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
