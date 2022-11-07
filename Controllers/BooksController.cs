using Library_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library_Management_System.Models.ViewModel;

namespace Library_Management_System.Controllers
{
    public class BooksController : MainController
    {
        public BooksController(LibraryContext context) : base(context) { }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<Book> library = await _context.Books.AsNoTracking().Include(b => b.Publisher).ToListAsync();
                List<BookCategoryViewModel> bookCategories = new List<BookCategoryViewModel>();

                foreach (Book book in library)
                {
                    List<Category> categories = await _context.BookCategories.AsNoTracking().Where(a => a.BookId == book.Id).Select(a => a.Category).ToListAsync();
                    BookCategoryViewModel bookCategoriesIndex = new BookCategoryViewModel { Book = book, Categories = categories };
                    bookCategories.Add(bookCategoriesIndex);
                }
                ViewBag.classId = 1;
                return View(bookCategories.ToList());
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (IsIdNull(id))
                    return NotFound();

                Book? book = await _context.Books.AsNoTracking().FirstOrDefaultAsync(pc => pc.Id == id);

                if (!IsContextValued(book))
                    return NotFound();

                List<Category> categories = _context.BookCategories.Where(a => a.BookId == book.Id).Select(a => a.Category).ToList();
                BookCategoryViewModel bookCategories = new BookCategoryViewModel { Book = book, Categories = categories };
                ViewBag.classId = 1;
                return View(bookCategories);
            }
            catch { throw new Exception(); }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
                ViewBag.CategoryId = new MultiSelectList(_context.Categories.ToList(), "Id", "Name");
                ViewBag.classId = 1;
                return View();
            }
            catch { throw new Exception(); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            
            if (!ModelState.IsValid)
            {
                ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
                ViewBag.CategoryId = new MultiSelectList(_context.Categories.ToList(), "Id", "Name");
                return View(book);
            }

            if (DoesItAlreadyExist(book.Id, book.Title))
            {
                ModelState.AddModelError("", "Book Already does ADD!");
                ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
                ViewBag.CategoryId = new MultiSelectList(_context.Categories.ToList(), "Id", "Name");
                return View(book);
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                transaction.CreateSavepoint("Before");
                book.Publisher = _context.Publishers.Where(p => p.Id == book.PublisherId).FirstOrDefault();
                List<BookCategory> bookCategories = new List<BookCategory>();
                List<string> categoryIds = Request.Form["CategoryId"].ToList();
                Category category = new Category();

                foreach (string id in categoryIds)
                {
                    category = _context.Categories.Where(a => a.Id == Convert.ToInt32(id)).FirstOrDefault();
                    BookCategory bookCategory = new BookCategory { Category = category, Book = book };
                    _context.BookCategories.Add(bookCategory);
                }
                _context.Books.Add(book);
                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                return BadRequest();
            }
            finally
            {
                _context.Dispose();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (IsIdNull(id)) return NotFound();

                Book? book = await _context.Books.FindAsync(id);

                if (!IsContextValued(book)) return NotFound();

                ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
                ViewBag.CategoryId = new MultiSelectList(_context.Categories.ToList(), "Id", "Name");
                ViewBag.classId = 1;
                BookCategoryViewModel bookCategoryViewModel = new BookCategoryViewModel { Book = book, ListItemCategory = new MultiSelectList(_context.Categories.ToList(), "Id", "Name"), SelectedCategories = _context.BookCategories.Where(e => e.BookId == id).Select(e => e.CategoryId).ToList() };
                BookCategory bookCategory = new BookCategory { Category = _context.BookCategories.Where(e => e.BookId == id).Select(e => e.Category).FirstOrDefault(), Book = book };
                return View(bookCategory);
            }
            catch { throw new Exception(); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            ViewBag.CategoryId = new MultiSelectList(_context.Categories.ToList(), "Id", "Name");
            
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            if (id != book.Id)
            {
                return NotFound();
            }

            if (DoesItAlreadyExist(book.Id, book.Title))
            {
                ModelState.AddModelError("", "Book Already does Exists");
                ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
                ViewBag.CategoryId = new MultiSelectList(_context.Categories.ToList(), "Id", "Name");
                return View(book);
            }

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                transaction.CreateSavepoint("Before");

                List<string> categoryIds = Request.Form["CategoryId"].ToList();

                _context.Books.Update(book);
                _context.SaveChanges();

                List<BookCategory> bookCategoriesFromDB = _context.BookCategories.Where(a => a.BookId == book.Id).ToList();
                List<BookCategory> bookCategoriesFromView = new List<BookCategory>();
                List<BookCategory> bookCategoriesFromDBCheck = new List<BookCategory>();

                bookCategoriesFromDBCheck.AddRange(bookCategoriesFromDB);

                foreach (string idCategoryFromView in categoryIds)
                {
                    BookCategory bookCategory = new BookCategory { Book = book, CategoryId = Convert.ToInt32(idCategoryFromView), BookId = book.Id };
                    bookCategoriesFromView.Add(bookCategory);
                }

                foreach (BookCategory bookCategory in bookCategoriesFromDBCheck)
                {
                    bool exist = bookCategoriesFromView.Any(a => a.CategoryId == bookCategory.CategoryId);
                    if (exist)
                    {
                        bookCategoriesFromView.Remove(bookCategoriesFromView.Where(a => a.CategoryId == bookCategory.CategoryId).First());
                        bookCategoriesFromDB.Remove(bookCategory);
                        
                    }
                }

                _context.BookCategories.RemoveRange(bookCategoriesFromDB);
                _context.BookCategories.AddRange(bookCategoriesFromView);
                _context.SaveChanges();

                transaction.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback();
                return BadRequest();
            }
            finally
            {
                _context.Dispose();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (IsIdNull(id)) return NotFound();

                Book? book = await _context.Books.AsNoTracking().Include(c => c.BookCategories).FirstOrDefaultAsync(c => c.Id == id);

                if (!IsContextValued(book)) return NotFound();

                ViewBag.classId = 1;
                return View(book);
            }
            catch { throw new Exception(); }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Book? book = await _context.Books.FindAsync(id);
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch { throw new Exception(); }
        }

        public bool IsContextValued(Book? book)
        {
            return book != null;
        }

        public bool DoesItAlreadyExist(int id, string title)
        {
            //_context.Books.AsNoTracking().Any(e => e.Id == id) ||
            return _context.Books.AsNoTracking().Any(e => e.Title == title);
        }
    }
}
