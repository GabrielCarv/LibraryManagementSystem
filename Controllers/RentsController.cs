using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library_Management_System.Controllers
{
    public class RentsController : MainController
    {
        public RentsController(LibraryContext context) : base(context){}

        public async Task<IActionResult> Index()
        {
            var rent = await _context.Rents.AsNoTracking().ToListAsync();
            ViewBag.classId = 0;
            return View(rent);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (IsIdNull(id))
                return NotFound();

            var rent = await _context.Rents.FirstOrDefaultAsync(r => r.Id == id);

            if (IsContextValued(rent))
                return NotFound();

            ViewBag.classId = 0;
            return View(rent);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
            ViewBag.classId = 0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RentalDate,RentReturnDate,RentRealReturnDate")] Rent rent)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
                ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                return View(rent);
            }

            if (DoesItAlreadyExist(rent.Id))
            {
                ModelState.AddModelError("", "Rent already does add!");
                ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
                ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                return View(rent);
            }
                
            return await DataBaseTransacion("insert", rent);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (IsIdNull(id))
                return NotFound();

            Rent? rent = await _context.Rents.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

            if (IsContextValued(rent))
                return NotFound();

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
            ViewBag.classId = 0;

            return View(rent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,RentalDate,RentReturnDate,RentRealReturnDate")] Rent rent)
        {
            if(!ModelState.IsValid)
            {
                ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
                ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                return View(rent);
            }

            if (id != rent.Id)
                return BadRequest();

            if (DoesItAlreadyExist(rent.Id))
                ModelState.AddModelError("", "Rent already does add!");

            return await DataBaseTransacion("update", rent);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (IsIdNull(id))
                return NotFound();

            var rent = await _context.Rents.AsNoTracking().FirstOrDefaultAsync(re => re.Id == id);

            if (IsContextValued(rent))
                return NotFound();

            ViewBag.classId = 0;
            return View(rent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Rent? rent = await _context.Rents.FindAsync(id);
            return await DataBaseTransacion("delete", rent);
        }

        private bool IsContextValued(Rent? rent)
        {
            return rent != null;
        }

        private bool DoesItAlreadyExist(int id)
        {
            return _context.Rents.AsNoTracking().Any(e => e.Id == id);
        }

        private async Task<IActionResult> DataBaseTransacion(string databaseCommand, Rent rent)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                transaction.CreateSavepoint("Before");

                switch (databaseCommand.ToLower())
                {
                    case "insert":
                        _context.Rents.Add(rent);
                        break;
                    case "update":
                        _context.Rents.Update(rent);
                        break;
                    case "delete":
                        _context.Rents.Remove(rent);
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
