using BookManagementSystem_BMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BookManagementSystem_BMS.Data;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem_BMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly BMSContext _dbContext;

        public HomeController(BMSContext dbContext)
        {
            _dbContext = dbContext;
        }

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

        public async Task<IActionResult> CategoryOverview(int categoryId)
        {
            // Get the selected category
            var category = await _dbContext.Categories.FindAsync(categoryId);

            if (category == null)
                return NotFound();

            // Get a list of books in the selected category
            var books = await _dbContext.Books
                .Include(b => b.Category)
                .Where(b => b.CategoryID == categoryId)
                .ToListAsync();

            // Pass the category and books to the view
            ViewBag.Category = category;
            ViewBag.Books = books;

            return View();
        }

        public async Task<IActionResult> BookOverview(int bookId)
        {
            // Get the selected book
            var book = await _dbContext.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookID == bookId);

            if (book == null)
                return NotFound();

            // Get a list of chapters for the selected book
            var chapters = await _dbContext.Chapters
                .Where(c => c.BookID == bookId)
                .ToListAsync();

            // Pass the book and chapters to the view
            ViewBag.Book = book;
            ViewBag.Chapters = chapters;

            return View();
        }

        public async Task<IActionResult> ChapterContent(int chapterId)
        {
            // Get the selected chapter
            var chapter = await _dbContext.Chapters.FindAsync(chapterId);

            if (chapter == null)
                return NotFound();

            // Pass the chapter to the view
            ViewBag.Chapter = chapter;

            return View();
        }
    }
}