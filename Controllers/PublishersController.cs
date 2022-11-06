using Library_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class PublishersController : MainController
    {
        public PublishersController(LibraryContext context) : base(context){}

        public async Task<IActionResult> Index()
        {
            ViewBag.classId = 4;
            return View(await _context.Publishers.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(IsIdNull(id))
                return NotFound();

            Publisher? publisher = await _context.Publishers.AsNoTracking().FirstOrDefaultAsync(pc => pc.Id == id);

            if(!IsContextValued(publisher))
              return NotFound();
            ViewBag.classId = 4;
            return View(publisher);
        }

        public IActionResult Create()
        {
            ViewBag.classId = 4;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Publisher publisher)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (DoesItAlreadyExist(publisher.Id, publisher.Name))
            {
                ModelState.AddModelError("", "Publisher already does exist!");
                return View(publisher);
            }

            return await DataBaseTransacion("insert", publisher);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (IsIdNull(id)) return NotFound();

            Publisher? publisher = await _context.Publishers.FindAsync(id);

            if (!IsContextValued(publisher)) return NotFound();
            ViewBag.classId = 4;
            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Publisher publisher)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id != publisher.Id)
                return BadRequest();

            if (DoesItAlreadyExist(publisher.Id, publisher.Name))
            {
                ModelState.AddModelError("", "Publisher already does add!");
                return View(publisher);
            }

            return await DataBaseTransacion("update", publisher);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (IsIdNull(id)) return NotFound();
            Publisher? publisher = await _context.Publishers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (!IsContextValued(publisher)) return NotFound();
            ViewBag.classId = 4;

            return View(publisher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Publisher? publisher = await _context.Publishers.FindAsync(id);
            return await DataBaseTransacion("delete", publisher);
        }

        private bool IsContextValued(Publisher? publisher)
        {
            return publisher != null;
        }

        private bool DoesItAlreadyExist(int id, string name)
        {
            //_context.Publishers.AsNoTracking().Any(e => e.Id == id) ||
            bool anyEquals =  _context.Publishers.AsNoTracking().Any(e => e.Name == name);
            return anyEquals;
        }

        private async Task<IActionResult> DataBaseTransacion(string databaseCommand, Publisher publisher)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                transaction.CreateSavepoint("Before");

                switch(databaseCommand.ToLower())
                {
                    case "insert":
                        _context.Publishers.Add(publisher);
                        break;
                    case "update":
                        _context.Publishers.Update(publisher);
                        break;
                    case "delete":
                        _context.Publishers.Remove(publisher);
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
