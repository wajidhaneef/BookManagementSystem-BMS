using BookManagementSystem_BMS.Data;
using BookManagementSystem_BMS.Dtos;
using BookManagementSystem_BMS.Models;
using BookManagementSystem_BMS.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookManagementSystem_BMS.Controllers
{
    public class BookController : Controller
    {
        private readonly BMSContext _dbContext;

        public BookController(BMSContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: BookController
        public async Task<IActionResult> Index(string bookSearch)
        {
            // Get a list of categories
            var categories = await _dbContext.Categories.ToListAsync();

            // Get a list of books with their respective categories
            var booksQuery = _dbContext.Books.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrEmpty(bookSearch))
            {
                booksQuery = booksQuery.Where(b => b.BookName.Contains(bookSearch));
            }

            var books = await booksQuery.ToListAsync();

            // Get a list of chapters for all books
            var chapters = await _dbContext.Chapters.ToListAsync();

            // Pass the categories, books, and chapters to the view
            ViewBag.Categories = categories;
            ViewBag.Books = books;
            ViewBag.Chapters = chapters;
            return View();
        }

        // GET: BookController/Details/5
        public ActionResult Details(int categoryId)
        {
            var categories = _dbContext.Categories.ToList();
            var viewModel = new BookViewModel
            {
                AllCategories = categories,
                SelectedCategory = categories.FirstOrDefault(b => b.CategoryID == categoryId, categories.First()).CategoryName,
                SelectedCategoryId = categoryId
            };

            return View("BookOverview",viewModel);
        }
        public ActionResult BookOverviewByCover (int bookId, int categoryId, int chapterId)
        {
            var categories = _dbContext.Categories.ToList();
            var books = _dbContext.Books.Where(b => b.CategoryID == categoryId).ToList();
            var chapters = _dbContext.Chapters.Where(b => b.BookID == bookId).ToList();
            var viewModel = new BookViewModel
            {
                AllCategories = categories,
                SelectedCategory = categories.FirstOrDefault(b => b.CategoryID == categoryId, categories.First()).CategoryName,
                SelectedCategoryId = categoryId,
                Books = books,
                SelectedBook = books.FirstOrDefault(b => b.BookID == bookId, books.First()).BookName,
                SelectedBookId = bookId != 0 ? bookId : books.First().BookID,
                Chapters = chapters,
                SelectedChapter = chapters.FirstOrDefault(b => b.ChapterID == chapterId, chapters.First()).ChapterName,
                SelectedChapterId = chapterId != 0 ? chapterId : chapters.First().ChapterID,

            };

            return View("BookOverview", viewModel);
        }
        // GET: BookController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: BookController/Edit/5
        /*[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(BookDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Category category = (Category)model.Category.Where(c => c.Selected == true);
        //        // Create a new Book entity
        //        var book = new Book
        //        {
        //            BookName = model.BookName,
        //            Category = category,
        //            CategoryID = category.CategoryID,
        //        };

        //        // Save the new book to the database
        //        _dbContext.Books.Add(book);
        //        _dbContext.SaveChanges();

        //        return RedirectToAction(nameof(Index));
        //    }

        //    // If the model state is not valid, return the Create view with the model to display validation errors
        //    return View(model);
        }*/

        // GET: BookController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/GetBooksByCategory
        public ActionResult GetBooksByCategory(int categoryId, int bookId)
        {
            //var books = _dbContext.Books.Where(b => b.CategoryID == categoryId).ToList();
            var books = _dbContext.Books.Where(b => b.CategoryID == categoryId).ToList();
            var viewModel = new BookViewModel
            {
                Books = books,
                SelectedBook = books.FirstOrDefault(b => b.BookID == bookId, books.First()).BookName,
                SelectedBookId = bookId!=0?bookId :books.First().BookID,
            };

            return PartialView("_BooksDropdown", viewModel);
        }
        
        // GET: Book/GetChaptersByBook
        public ActionResult GetChaptersByBook(int bookId, int chapterId)
        {
            //var chapters = _dbContext.Chapters.Where(c => c.BookID == bookId).ToList();
            var chapters = _dbContext.Chapters.Where(b => b.BookID == bookId).ToList();
            var viewModel = new BookViewModel
            {
                Chapters = chapters,
                SelectedChapter = chapters.FirstOrDefault(b => b.ChapterID == chapterId, chapters.First()).ChapterName,
                SelectedChapterId = chapterId!=0?chapterId: chapters.First().ChapterID,
            };

            return PartialView("_ChaptersDropdown", viewModel);
        }

        // GET: Book/GetChapterContent
        public ActionResult GetChapterContent(int chapterId)
        {
            var chapter = _dbContext.Chapters.FirstOrDefault(c => c.ChapterID == chapterId);

            if (chapter != null)
            {
                return Content(chapter.Content);
            }

            return Content(string.Empty);
        }

    }
}
