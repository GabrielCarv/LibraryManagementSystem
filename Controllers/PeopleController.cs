using Library_Management_System.Data;
using Library_Management_System.Models;
using Library_Management_System.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Library_Management_System.Controllers
{
    public class PeopleController : MainController
    {
        public PeopleController(LibraryContext context) : base(context){}

        public async Task<IActionResult> Index()
        {
            try
            {
                List<Person> people = await _context.People.AsNoTracking().ToListAsync();
                List<PeoplePhoneViewModel> peoplePhones = new List<PeoplePhoneViewModel>();

                foreach (Person person in people)
                {
                    PeoplePhoneViewModel peoplePhonesIndex = new PeoplePhoneViewModel { Person = person, Phone = _context.Phones.Where(a => a.PersonCpf == person.Cpf).AsNoTracking().FirstOrDefault() };
                    peoplePhones.Add(peoplePhonesIndex);
                }
                ViewBag.classId = 2;
                return View(peoplePhones);
            }
            catch { throw new Exception(); }
        }

        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                if (IsCpfNull(id))
                    return NotFound();

                var person = await _context.People.AsNoTracking().FirstOrDefaultAsync(p => p.Cpf == id);

                if (IsContextValued(person))
                    return NotFound();
                ViewBag.classId = 2;
                PeoplePhoneViewModel personPhones = new PeoplePhoneViewModel { Person = person, Phone = _context.Phones.Where(a => a.Person == person).AsNoTracking().FirstOrDefault() };
                return View(personPhones);
            }
            catch { throw new Exception(); }
        }
        public IActionResult Create()
        {
            try
            {
                ViewBag.classId = 2;
                return View();
            }
            catch { throw new Exception(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(person);

                if (DoesItAlreadyExist(person.Cpf))
                {
                    ModelState.AddModelError("", "Person already does add!");
                    return View(person);
                }

                return await DataBaseTransacion("insert", person);
            }
            catch { throw new Exception(); }

        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (IsCpfNull(id))
                    return NotFound();

                Person? person = await _context.People.FindAsync(id);

                if (!IsContextValued(person))
                    return NotFound();

                ViewBag.classId = 2;
                PeoplePhoneViewModel personPhones = new PeoplePhoneViewModel { Person = person, Phone = _context.Phones.Where(a => a.Person == person).AsNoTracking().FirstOrDefault() };
                return View(personPhones);
            }
            catch { throw new Exception(); }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(person);

                if (id != person.Cpf)
                    return NotFound();

                return await DataBaseTransacion("update", person);
            }
            catch { throw new Exception(); }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {

                if (IsCpfNull(id))
                    return NotFound();

                Person? person = await _context.People.AsNoTracking().FirstOrDefaultAsync(m => m.Cpf == id);

                if (IsContextValued(person))
                    return NotFound();
                ViewBag.classId = 2;
                return View(person);
            }
            catch { throw new Exception(); }
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                Person? person = await _context.People.AsNoTracking().FirstOrDefaultAsync(m => m.Cpf == id);
                return await DataBaseTransacion("delete", person);
            }
            catch { throw new Exception(); }

        }

        private bool IsContextValued(Person? person)
        {
            return person != null;
        }

        private bool DoesItAlreadyExist(string cpf)
        {
            return _context.People.AsNoTracking().Any(e => e.Cpf == cpf.ToString());
        }

        private bool IsCpfNull(string? cpf)
        {
            return cpf == null;
        }

        private async Task<IActionResult> DataBaseTransacion(string databaseCommand, Person person)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                transaction.CreateSavepoint("Before");

                //string password = person.Password;
                //person.Password = EncryptPassword(person.Cpf, password);

                switch (databaseCommand.ToLower())
                {
                    case "insert":
                        _context.People.Add(person);
                        break;
                    case "update":
                        _context.People.Update(person);
                        break;
                    case "delete":
                        _context.People.Remove(person);
                        break;
                    default:
                        throw new Exception("Please name a correct database command");
                }
                await _context.SaveChangesAsync();

                List<string> numbers = Request.Form["Phone"].ToList();
               
                foreach (string phoneNumber in numbers)
                {
                    foreach (char number in phoneNumber)
                    {
                        try
                        {
                            int pNumber = (int)Convert.ToInt64(number);
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    }
                    Phone phone = new Phone(phoneNumber, person.Cpf);

                    switch (databaseCommand.ToLower())
                    {
                        case "insert":
                            _context.Phones.Add(phone);
                            _context.SaveChanges();
                            break;
                        case "update":
                            List<Phone> phonesFromView = new List<Phone>();
                            List<Phone> phonesFromDB = _context.Phones.Where(e => e.PersonCpf == person.Cpf).ToList();
                            phonesFromView.Add(phone);
                            
                            foreach (Phone p in phonesFromDB)
                                _context.Phones.Remove(p);

                            foreach (Phone p in phonesFromView)
                                _context.Phones.Add(p);
                            
                            break;
                        case "delete":
                            _context.Phones.Remove(phone);
                            break;
                        default:
                            throw new Exception("Please name a correct database command");
                    }
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
