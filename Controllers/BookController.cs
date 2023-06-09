﻿using BookManagementSystem_BMS.Data;
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
            var categories = _dbContext.Categories.ToList();
            var books = _dbContext.Books.ToList();

            var coverpages = _dbContext.CoverPages.ToList();
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
                    CoverImages.Add(new CoverPageViewModel{
                        //CoverImage = MyBitmap, BookId=coverPage.BookId,
                        BookName= books.FirstOrDefault(b=>b.BookID==coverPage.BookId).BookName,
                        CategoryId = books.FirstOrDefault(b => b.BookID == coverPage.BookId).CategoryID,
                        ImageContent = imgBytes
                    });
                }
            }
            ViewBag.LoggedIn = loggedin;
            var viewModel = new BookViewModel
            {
                AllCategories = categories,
                Books = books,
                CoverPages = CoverImages,
                

            };
            return View(viewModel);
            
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
        public ActionResult BookOverviewByCover(int bookId, int categoryId, int chapterId, int home)
        {
            var categories = _dbContext.Categories.ToList();
            var chapters = _dbContext.Chapters.ToList();
            var SelectedBookId = 0;
            var SelectedChapterId = 0;
            if (home==1)
            {
                categoryId = _dbContext.Books.FirstOrDefault(b => b.BookID == bookId).CategoryID;
            }
            else
            {
                SelectedBookId = _dbContext.Books.FirstOrDefault(b => b.CategoryID == categoryId).BookID;
                SelectedChapterId = chapters.FirstOrDefault(b => b.BookID == bookId, chapters.First()).ChapterID;
            }
            
            
            var viewModel = new BookViewModel
            {
                AllCategories = categories,
                SelectedCategory = categories.FirstOrDefault(b => b.CategoryID == categoryId, categories.First()).CategoryName,
                SelectedCategoryId = categoryId,
                SelectedBookId = bookId != 0 ? bookId : SelectedBookId,
                SelectedChapterId = chapterId != 0 ? chapterId : SelectedChapterId,

            };

            return View("BookOverview", viewModel);
            /*var categories = _dbContext.Categories.ToList();
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
                SelectedChapterContent = chapters.FirstOrDefault(b => b.ChapterID == chapterId, chapters.First()).Content,

            };*/

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
                    return RedirectToAction("Index");
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
