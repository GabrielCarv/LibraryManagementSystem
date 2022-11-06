using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library_Management_System.Controllers
{
    public class RentsController : MainController
    {
        public RentsController(LibraryContext context) : base(context) { }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<Rent?> rents = await _context.Rents.AsNoTracking().ToListAsync();
                foreach (Rent rent in rents)
                {
                    rent.Property = _context.Properties.AsNoTracking().Where(a => a.Id == rent.PropertiesId).FirstOrDefault();
                    rent.Person = _context.People.AsNoTracking().Where(a => a.Cpf == rent.PersonId).FirstOrDefault();
                }
                ViewBag.classId = 0;
                return View(rents);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Detail(int? id)
        {
            try
            {
                if (IsIdNull(id))
                    return NotFound();

                var rent = await _context.Rents.FirstOrDefaultAsync(r => r.Id == id);

                if (IsContextValued(rent))
                    return NotFound();

                ViewBag.classId = 0;
                return View(rent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                ViewData["BookId"] = new SelectList(_context.Properties, "Id", "Id");
                ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                ViewBag.classId = 0;
                return View();
            }
            catch (Exception ex) { throw new Exception(); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rent rent)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["BookId"] = new SelectList(_context.Properties, "Id", "Id");
                    ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                    return View(rent);
                }

                if (DoesItAlreadyExist(rent.Id, rent.PropertiesId))
                {
                    ModelState.AddModelError("", "Rent already does add!");
                    ViewData["BookId"] = new SelectList(_context.Properties, "Id", "Id");
                    ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                    return View(rent);
                }

                return await DataBaseTransacion("insert", rent);
            }
            catch (Exception ex) { throw ex; }

        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (IsIdNull(id))
                    return NotFound();

                Rent? rent = await _context.Rents.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

                if (IsContextValued(rent))
                    return NotFound();

                ViewData["BookId"] = new SelectList(_context.Books, "Id", "Tittle");
                ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                ViewBag.classId = 0;

                return View(rent);
            }
            catch (Exception ex) { throw ex; }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,RentalDate,RentReturnDate,RentRealReturnDate")] Rent rent)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["BookId"] = new SelectList(_context.Books, "Id", "Tittle");
                    ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                    return View(rent);
                }

                if (id != rent.Id)
                    return BadRequest();

                if (DoesItAlreadyExist(rent.Id, rent.PropertiesId))
                {
                    ModelState.AddModelError("", "Rent already does add!");
                    ViewData["BookId"] = new SelectList(_context.Properties, "Id", "Id");
                    ViewData["PersonCpf"] = new SelectList(_context.People, "Cpf", "Name");
                    return View(rent);
                }

                return await DataBaseTransacion("update", rent);
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
                if (IsIdNull(id))
                    return NotFound();

                var rent = await _context.Rents.AsNoTracking().FirstOrDefaultAsync(re => re.Id == id);

                if (IsContextValued(rent))
                    return NotFound();

                ViewBag.classId = 0;
                return View(rent);
            }
            catch (Exception ex) { throw ex; }
           
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

        private bool DoesItAlreadyExist(int id, int propertyid)
        {
            // _context.Rents.AsNoTracking().Any(e => e.Id == id) ||
            bool anyExist = _context.Rents.AsNoTracking().Any(e => e.PropertiesId == propertyid);
            return anyExist;
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
                _context.SaveChanges();
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
