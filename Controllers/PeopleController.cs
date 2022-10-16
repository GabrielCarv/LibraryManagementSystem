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
            List<Person> people = await _context.People.AsNoTracking().ToListAsync();
            List<PeoplePhoneViewModel> peoplePhones = new List<PeoplePhoneViewModel>();

            foreach(Person person in people)
            {
                List<Phone> phones = await _context.Phones.Where(a => a.Person == person).AsNoTracking().ToListAsync();
                PeoplePhoneViewModel peoplePhonesIndex = new PeoplePhoneViewModel { Person = person, Phones = phones };
                peoplePhones.Add(peoplePhonesIndex);
            }
            ViewBag.classId = 2;
            return View(peoplePhones);
        }

        public async Task<IActionResult> Details(string? cpf)
        {
            if(IsCpfNull(cpf))
                return NotFound();

            var person = await _context.People.AsNoTracking().FirstOrDefaultAsync(p => p.Cpf == cpf);

            if (IsContextValued(person))
                return NotFound();
            ViewBag.classId = 2;
            List<Phone> phones = await _context.Phones.Where(a => a.Person == person).AsNoTracking().ToListAsync();
            PeoplePhoneViewModel personPhones = new PeoplePhoneViewModel { Person = person, Phones = phones };
            return View(personPhones);
        }
        public IActionResult Create()
        {
            ViewBag.classId = 2;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cpf,Name,Email,IsEmployer,PostalCode,State,City,Address,HouseNumber")] Person person)
        {
            if(!ModelState.IsValid)
                return View(person);

            if(DoesItAlreadyExist(person.Cpf))
                ModelState.AddModelError("", "Person already does add!");

            return await DataBaseTransacion("insert", person);
        }

        public async Task<IActionResult> Edit([FromRoute]string id)
        {
            if (IsCpfNull(id))
                return NotFound();

            Person? person = await _context.People.FindAsync(id);

            if (!IsContextValued(person))
                return NotFound();

            ViewBag.classId = 2;
            List<Phone> phones = await _context.Phones.Where(a => a.Person.Cpf == id).AsNoTracking().ToListAsync();
            PeoplePhoneViewModel personPhones = new PeoplePhoneViewModel { Person = person, Phones = phones };
            return View(personPhones);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string cpf, [Bind("Cpf,Name,Email,IsEmployer,PostalCode,State,City,Address,HouseNumber")] Person person)
        {
            if (ModelState.IsValid)
                return View(person);
            if (cpf != person.Cpf)
                return NotFound();

            return await DataBaseTransacion("update", person);
        }

        public async Task<IActionResult> Delete(string cpf)
        {
            if (IsCpfNull(cpf))
                return NotFound();

            Person? person = await _context.People.AsNoTracking().FirstOrDefaultAsync(m => m.Cpf == cpf);

            if (IsContextValued(person))
                return NotFound();
            ViewBag.classId = 2;
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string cpf)
        {
            Person? person = await _context.People.AsNoTracking().FirstOrDefaultAsync(m => m.Cpf == cpf);
            return await DataBaseTransacion("delete", person);
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

                List<string> numbers = Request.Form["Number"].ToList();
               
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
                    Phone phone = new Phone { PhoneNumber = phoneNumber, Person = person };

                    switch (databaseCommand.ToLower())
                    {
                        case "insert":
                            _context.Phones.Add(phone);
                            break;
                        case "update":
                            List<Phone> phonesFromView = new List<Phone>();
                            List<Phone> phonesFromDB = _context.Phones.Where(e => e.Person.Cpf == person.Cpf).ToList();
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
