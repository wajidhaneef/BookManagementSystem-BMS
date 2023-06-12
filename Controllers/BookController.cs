using BookManagementSystem_BMS.Data;
using BookManagementSystem_BMS.Dtos;
using BookManagementSystem_BMS.Models;
using BookManagementSystem_BMS.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Claims;
using static System.Reflection.Metadata.BlobBuilder;

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
        public IActionResult Index(string loggedin)
        {
            //find the logged in user role
            string strRoleId = User.FindFirstValue(ClaimTypes.Role);
            // parse the id
            if (!string.IsNullOrEmpty(strRoleId))
            {
                int roleId = int.Parse(strRoleId);
                var role = _dbContext.Roles
                    .Include(r => r.Categories) 
                    .FirstOrDefault(r => r.RoleID == roleId);
                var categoriesAssociated = role.Categories;

                var categoryIds = categoriesAssociated.Select(c => c.CategoryID).ToList();
                //var categories = _dbContext.Categories.ToList();
                var books = _dbContext.Books
                    .Include(b => b.Category)
                    .Where(b => categoryIds.Contains(b.CategoryID))
                    .ToList();
                var selectedBook = books.First();
                var chapters = _dbContext.Chapters.Where(c=>c.BookID==selectedBook.BookID).ToList();
                var bookIds = books.Select(b=>b.BookID).ToList();

                var coverpages = _dbContext.CoverPages
                    .Include(cp => cp.Book)
                    .Where(cp => bookIds.Contains(cp.BookId))
                    .ToList();

                List<CoverPageViewModel> CoverImages = new List<CoverPageViewModel>();
                foreach (var coverPage in coverpages)
                {
                    if (coverPage != null)
                    {
                        // Create a memory stream from the image data
                        //var arr = coverPage.ImageData.ToArray();
                        //var stream = new MemoryStream(coverPage.ImageData);
                        byte[] imgBytes = (byte[])coverPage.ImageData;

                        //If you want convert to a bitmap file
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                        //Bitmap MyBitmap = (Bitmap)tc.ConvertFrom(imgBytes);
                        // Set the content type based on the file extension
                        //var contentType = GetContentType(coverPage.ImageMimeType);
                        //Image img = Image.FromStream(stream, false, true);
                        // Return the image as a file result
                        CoverImages.Add(new CoverPageViewModel
                        {
                            //CoverImage = MyBitmap, BookId=coverPage.BookId,
                            BookName = books.FirstOrDefault(b => b.BookID == coverPage.BookId).BookName,
                            CategoryId = books.FirstOrDefault(b => b.BookID == coverPage.BookId).CategoryID,
                            ImageContent = imgBytes,
                            BookId = coverPage.BookId
                        });
                    }
                }
                Random random = new Random();
                CoverImages.Sort((x, y) => random.Next(-1, 2));

                ViewBag.LoggedIn = loggedin;
                var viewModel = new BookViewModel
                {
                    AllCategories = categoriesAssociated,
                    Books = books,
                    Chapters = chapters,
                    SelectedBook = selectedBook.BookName,
                    SelectedCategory = categoriesAssociated.First().CategoryName,
                    SelectedCategoryId = categoriesAssociated.First().CategoryID,
                    SelectedBookId = selectedBook.BookID,
                    SelectedChapterId = chapters.First().ChapterID,
                    SelectedChapter = chapters.First().ChapterName,
                    SelectedChapterContent = chapters.First().Content,
                    CoverPages = CoverImages,
                    //Roles = roles,
                    //SelectedRoleId = roles.First().RoleID,


                };
                return View(viewModel);
            }
            else
            {
                var dummyCategories = DummyDataGenerator.GenerateCategories();
                var dummyBooks = DummyDataGenerator.GenerateBooks();
                List<CoverPageViewModel> CoverImages = new List<CoverPageViewModel>();
                foreach (var coverPage in DummyDataGenerator.GenerateCoverPages())
                {
                    if (coverPage != null)
                    {
                        
                        CoverImages.Add(new CoverPageViewModel
                        {
                            //CoverImage = MyBitmap, BookId=coverPage.BookId,
                            BookName = dummyBooks.FirstOrDefault(b => b.BookID == coverPage.BookId).BookName,
                            CategoryId = dummyBooks.FirstOrDefault(b => b.BookID == coverPage.BookId).CategoryID,
                            ImageContent = coverPage.ImageData,
                            BookId = coverPage.BookId
                        });
                    }
                }
                var dummyModel = new BookViewModel
                {
                    AllCategories = dummyCategories,
                    Books = dummyBooks,
                    Chapters = DummyDataGenerator.GenerateChapters(),
                    
                    SelectedCategoryId = 1,
                    SelectedBookId = 1,
                    SelectedChapterId = 1,
                    
                    SelectedChapterContent = DummyDataGenerator.GenerateChapters().First().Content,
                    CoverPages = CoverImages,
                };
                return View(dummyModel);
            }
            
            
        }
        

        // GET: BookController/Details/5
        public ActionResult Details(BookViewModel viewModel, int categoryId)
        {
            //find the logged in user role
            string strRoleId = User.FindFirstValue(ClaimTypes.Role);
            // parse the id
            if (!string.IsNullOrEmpty(strRoleId))
            {
                var books = _dbContext.Books.Where(b => b.CategoryID == categoryId).ToList();
                int bookId = books.First().BookID;
                var chapters = _dbContext.Chapters.Where(c => c.BookID == bookId).ToList();

                ViewBag.Roles = _dbContext.Roles.ToList();

                var categories = _dbContext.Categories.ToList();
                viewModel.AllCategories = categories;
                viewModel.Books = books;
                viewModel.SelectedCategoryId = categoryId;
                viewModel.SelectedBookId = bookId;
                viewModel.Chapters = chapters;
                viewModel.SelectedChapterId = chapters.First().ChapterID;
                viewModel.SelectedChapterContent = chapters.First().Content;


                return View("BookOverview", viewModel);
            }
            else
            {
                var books = DummyDataGenerator.GenerateBooks().Where(b => b.CategoryID == categoryId).ToList();
                int bookId = books.First().BookID;
                var chapters = DummyDataGenerator.GenerateChapters().Where(c => c.BookID == bookId).ToList();


                var categories = DummyDataGenerator.GenerateCategories().ToList();
                viewModel.AllCategories = categories;
                viewModel.Books = books;
                viewModel.SelectedCategoryId = categoryId;
                viewModel.SelectedBookId = bookId;
                viewModel.Chapters = chapters;
                viewModel.SelectedChapterId = chapters.First().ChapterID;
                viewModel.SelectedChapterContent = chapters.First().Content;


                return View("BookOverview", viewModel);
            }
            
        }
        
        [HttpGet]
        public ActionResult BookOverviewByCover(int bookId, int categoryId, int chapterId)
        {
            //find the logged in user role
            string strRoleId = User.FindFirstValue(ClaimTypes.Role);
            // parse the id
            if (!string.IsNullOrEmpty(strRoleId))
            {
                var categories = _dbContext.Categories.ToList();
                var books = _dbContext.Books.Where(b => b.CategoryID == categoryId).ToList();
                var chapters = _dbContext.Chapters.Where(c => c.BookID == bookId).ToList();


                var viewModel = new BookViewModel
                {
                    AllCategories = categories,
                    Books = books,
                    Chapters = chapters,
                    SelectedCategoryId = categoryId,
                    SelectedBookId = bookId,
                    SelectedChapterId = chapterId == 0 ? chapters.First().ChapterID : chapterId,
                    SelectedChapterContent = chapters.FirstOrDefault(c => c.ChapterID == chapterId, chapters.First()).Content,
                    //Roles = roles,
                    //SelectedRoleId = roles.First().RoleID

                };

                return View("BookOverview", viewModel);
            }
            else
            {
                var categories = DummyDataGenerator.GenerateCategories().ToList();
                var books = DummyDataGenerator.GenerateBooks().Where(b => b.CategoryID == categoryId).ToList();
                var chapters = DummyDataGenerator.GenerateChapters().Where(c => c.BookID == bookId).ToList();


                var viewModel = new BookViewModel
                {
                    AllCategories = categories,
                    Books = books,
                    Chapters = chapters,
                    SelectedCategoryId = categoryId,
                    SelectedBookId = bookId,
                    SelectedChapterId = chapterId == 0 ? chapters.First().ChapterID : chapterId,
                    SelectedChapterContent = chapters.FirstOrDefault(c => c.ChapterID == chapterId, chapters.First()).Content,
                    //Roles = roles,
                    //SelectedRoleId = roles.First().RoleID

                };

                return View("BookOverview", viewModel);
            }
        }

        [HttpGet]
        public ActionResult BookOverview(BookViewModel viewModel)
        {
            //find the logged in user role
            string strRoleId = User.FindFirstValue(ClaimTypes.Role);
            // parse the id
            if (!string.IsNullOrEmpty(strRoleId))
            {
                viewModel.AllCategories = _dbContext.Categories.ToList();
                var books = _dbContext.Books.ToList();
                viewModel.Books = books;
                var chapters = _dbContext.Chapters.Where(c => c.BookID == viewModel.SelectedBookId).ToList();
                if (chapters == null)
                {
                    var bookId = books.First().BookID;
                    chapters = _dbContext.Chapters.Where(c => c.BookID == bookId).ToList();
                }
                viewModel.Chapters = chapters;
                viewModel.SelectedChapterContent = viewModel.Chapters.FirstOrDefault(c => c.ChapterID == viewModel.SelectedChapterId, viewModel.Chapters.First()).Content;

                return View("BookOverview", viewModel);
            }
            else
            {
                viewModel.AllCategories = DummyDataGenerator.GenerateCategories().ToList();
                var books = DummyDataGenerator.GenerateBooks().ToList();
                viewModel.Books = books;
                var chapters = DummyDataGenerator.GenerateChapters().Where(c => c.BookID == viewModel.SelectedBookId).ToList();
                if (chapters == null)
                {
                    var bookId = books.First().BookID;
                    chapters = DummyDataGenerator.GenerateChapters().Where(c => c.BookID == bookId).ToList();
                }
                viewModel.Chapters = chapters;
                viewModel.SelectedChapterContent = DummyDataGenerator.GenerateChapters().FirstOrDefault(c => c.ChapterID == viewModel.SelectedChapterId, viewModel.Chapters.First()).Content;

                return View("BookOverview", viewModel);
            }
        }
        // GET: Book/GetBooksByCategory
        public ActionResult GetBooksByCategory(BookViewModel viewModel, int categoryId)
        {
            //find the logged in user role
            string strRoleId = User.FindFirstValue(ClaimTypes.Role);
            // parse the id
            if (!string.IsNullOrEmpty(strRoleId))
            {
                viewModel.Books = _dbContext.Books.Where(b => b.CategoryID == categoryId).ToList();
                viewModel.SelectedBookId = viewModel.Books.First().BookID;

                ViewBag.Roles = _dbContext.Roles.ToList();

                viewModel.SelectedRoleId = viewModel.Roles.First().RoleID;
                
                return PartialView("_BooksDropdown", viewModel);
            }
            else
            {
                viewModel.Books = DummyDataGenerator.GenerateBooks().Where(b => b.CategoryID == categoryId).ToList();
                viewModel.SelectedBookId = viewModel.Books.First().BookID;

                return PartialView("_BooksDropdown", viewModel);
            }
        }
        
        // GET: Book/GetChaptersByBook
        public ActionResult GetChaptersByBook(BookViewModel viewModel, int bookId, int categoryId)
        {
            //find the logged in user role
            string strRoleId = User.FindFirstValue(ClaimTypes.Role);
            // parse the id
            if (!string.IsNullOrEmpty(strRoleId))
            {
                if (categoryId == 0)
                {
                    categoryId = _dbContext.Books.First(b => b.BookID == bookId).CategoryID;
                }
                viewModel.Chapters = _dbContext.Chapters.Where(c => c.BookID == bookId).ToList();
                viewModel.SelectedChapterId = viewModel.Chapters.First().ChapterID;
                viewModel.SelectedChapterContent = viewModel.Chapters.First().Content;
                viewModel.SelectedBookId = bookId;
                viewModel.SelectedCategoryId = categoryId;

                return PartialView("_ChaptersDropdown", viewModel);
            }
            else
            {
                if (categoryId == 0)
                {
                    categoryId = DummyDataGenerator.GenerateBooks().First(b => b.BookID == bookId).CategoryID;
                }
                viewModel.Chapters = DummyDataGenerator.GenerateChapters().Where(c => c.BookID == bookId).ToList();
                viewModel.SelectedChapterId = viewModel.Chapters.First().ChapterID;
                viewModel.SelectedChapterContent = viewModel.Chapters.First().Content;
                viewModel.SelectedBookId = bookId;
                viewModel.SelectedCategoryId = categoryId;

                return PartialView("_ChaptersDropdown", viewModel);
            }
            
        }

        // GET: Book/GetChapterContent
        public ActionResult GetChapterContent(BookViewModel viewModel, int chapterId)
        {
            //find the logged in user role
            string strRoleId = User.FindFirstValue(ClaimTypes.Role);
            // parse the id
            if (!string.IsNullOrEmpty(strRoleId))
            {
                var content = _dbContext.Chapters.FirstOrDefault(c => c.ChapterID == chapterId).Content;

                if (content != null)
                {
                    return Content(content);
                }

                return Content(string.Empty);
            }
            else
            {
                var content = DummyDataGenerator.GenerateChapters().FirstOrDefault(c => c.ChapterID == chapterId).Content;

                if (content != null)
                {
                    return Content(content);
                }

                return Content(string.Empty);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UploadCoverPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadCoverPage(IFormFile coverPageFile, int bookId)
        {
            if (coverPageFile != null && coverPageFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await coverPageFile.CopyToAsync(stream);
                    var imageData = stream.ToArray();

                    // Get the file extension
                    var fileExtension = Path.GetExtension(coverPageFile.FileName).ToLower();

                    // Save the image data and file extension to the database
                    var coverPage = new CoverPage
                    {
                        ImageData = imageData,
                        ImageMimeType = fileExtension,
                        BookId = bookId,
                    };

                    // Assuming you have an instance of your database context named `_dbContext`
                    _dbContext.CoverPages.Add(coverPage);
                    await _dbContext.SaveChangesAsync();
                    var book = await _dbContext.Books.FirstOrDefaultAsync(c => c.BookID == bookId);
                    //book.CoverPageId = coverPage.Id;
                    //_dbContext.Update(book);
                    await _dbContext.SaveChangesAsync();
                    // Return a success message or redirect to another page
                    return View();
                }
            }

            // Handle invalid or missing file
            return RedirectToAction("Error");
        }

        /////////////////////////////////////////////////////////////
        public IActionResult ShowCoverPage(int coverPageId)
        {
            var coverPage = _dbContext.CoverPages.FirstOrDefault(cp => cp.Id == coverPageId);
            if (coverPage != null)
            {
                // Create a memory stream from the image data
                var stream = new MemoryStream(coverPage.ImageData);

                // Set the content type based on the file extension
                var contentType = GetContentType(coverPage.ImageMimeType);

                // Return the image as a file result
                return File(stream, contentType);
            }

            // Handle if cover page is not found
            return RedirectToAction("Error");
        }

        private string GetContentType(string fileExtension)
        {
            // Define content types for common image file extensions
            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                // Add more cases for other file extensions if needed
                default:
                    return "application/octet-stream";
            }
        }

    }
}
