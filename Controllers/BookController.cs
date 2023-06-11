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
using System.Security.Claims;

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
            //find the logged in user
            string strRoleId = User.FindFirstValue(ClaimTypes.Role);
            // parse the id
            //int roleId = int.Parse(strRoleId);
            // find the user role id from users table
            //int userRoleID = _dbContext.Users.FirstOrDefault(x => x.UserID == userId).RoleID;
            // find the category id associated with the role
            //var userCategories = _dbContext.Roles.FirstOrDefault(r => r.RoleID == roleId).Categories;

            var categories = _dbContext.Categories.ToList();
            var books = _dbContext.Books.ToList();
            var chapters = _dbContext.Chapters.ToList();
            var coverpages = _dbContext.CoverPages.ToList();
            ViewBag.Roles = _dbContext.Roles.ToList();
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
                AllCategories = categories,
                Books = books,
                Chapters = chapters,
                SelectedBook = books.First().BookName,
                SelectedCategory = categories.First().CategoryName,
                SelectedCategoryId = categories.First().CategoryID,
                SelectedBookId = books.First().BookID,
                SelectedChapterId = chapters.First().ChapterID,
                SelectedChapter = chapters.First().ChapterName,
                SelectedChapterContent = chapters.First().Content,
                CoverPages = CoverImages,
                //Roles = roles,
                //SelectedRoleId = roles.First().RoleID,
                

            };
            return View(viewModel);
            
        }
        

        // GET: BookController/Details/5
        public ActionResult Details(BookViewModel viewModel, int categoryId)
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
            //viewModel.Roles = roles;
            //viewModel.SelectedRoleId = roles.First().RoleID;
            //var viewModel = new BookViewModel
            //{
            //    AllCategories = categories,
            //    SelectedCategory = categories.FirstOrDefault(b => b.CategoryID == categoryId, categories.First()).CategoryName,
            //    SelectedCategoryId = categoryId
            //};

            return View("BookOverview",viewModel);
        }
        //[HttpGet]
        //public ActionResult BookOverviewByCover(IFormCollection collection)
        //{
        //    string selectedChapter = collection["username"];
        //    var viewModel = new BookViewModel();
        //    Console.WriteLine(selectedChapter);
        //    return View("BookOverview");
        //}
        [HttpGet]
        public ActionResult BookOverviewByCover(int bookId, int categoryId, int chapterId)
        {
            var categories = _dbContext.Categories.ToList();
            var books = _dbContext.Books.Where(b=>b.CategoryID==categoryId).ToList();
            var chapters = _dbContext.Chapters.Where(c=>c.BookID==bookId).ToList();

            //ViewBag.Roles = _dbContext.Roles.ToList();

            var viewModel = new BookViewModel
            {
                AllCategories = categories,
                Books = books,
                Chapters = chapters,
                SelectedCategoryId = categoryId,
                SelectedBookId = bookId,
                SelectedChapterId = chapterId == 0 ? chapters.First().ChapterID : chapterId,
                SelectedChapterContent = chapters.FirstOrDefault(c=>c.ChapterID==chapterId, chapters.First()).Content,
                //Roles = roles,
                //SelectedRoleId = roles.First().RoleID

            };

            return View("BookOverview", viewModel);
        }

        [HttpGet]
        public ActionResult BookOverview(BookViewModel viewModel)
        {
            viewModel.AllCategories = _dbContext.Categories.ToList();
            viewModel.Books = _dbContext.Books.ToList();
            viewModel.Chapters = _dbContext.Chapters.Where(c=>c.BookID==viewModel.SelectedBookId).ToList();
            viewModel.SelectedChapterContent = viewModel.Chapters.FirstOrDefault(c => c.ChapterID == viewModel.SelectedChapterId, viewModel.Chapters.First()).Content;


            return View("BookOverview", viewModel);
        }
        // GET: Book/GetBooksByCategory
        public ActionResult GetBooksByCategory(BookViewModel viewModel, int categoryId)
        {
            viewModel.Books = _dbContext.Books.Where(b => b.CategoryID == categoryId).ToList();
            viewModel.SelectedBookId = viewModel.Books.First().BookID;

            ViewBag.Roles = _dbContext.Roles.ToList();

            viewModel.SelectedRoleId = viewModel.Roles.First().RoleID;
            //var books = _dbContext.Books.Where(b => b.CategoryID == categoryId).ToList();
            //var viewModel = new BookViewModel
            //{
            //    Books = books,
            //    SelectedBook = books.FirstOrDefault(b => b.BookID == bookId, books.First()).BookName,
            //    SelectedBookId = bookId!=0?bookId :books.First().BookID,
            //};

            return PartialView("_BooksDropdown", viewModel);
        }
        
        // GET: Book/GetChaptersByBook
        public ActionResult GetChaptersByBook(BookViewModel viewModel, int bookId, int categoryId)
        {
            if (categoryId == 0)
            {
                categoryId = _dbContext.Books.First(b=>b.BookID == bookId).CategoryID;
            }
            viewModel.Chapters = _dbContext.Chapters.Where(c => c.BookID == bookId).ToList();
            viewModel.SelectedChapterId = viewModel.Chapters.First().ChapterID;
            viewModel.SelectedChapterContent = viewModel.Chapters.First().Content;
            viewModel.SelectedBookId = bookId;
            viewModel.SelectedCategoryId = categoryId;

            //var viewModel = new BookViewModel
            //{
            //    Chapters = chapters,
            //    SelectedChapter = chapters.FirstOrDefault(b => b.ChapterID == chapterId, chapters.First()).ChapterName,
            //    SelectedChapterId = chapterId!=0?chapterId: chapters.First().ChapterID,
            //};

            return PartialView("_ChaptersDropdown", viewModel);
        }

        // GET: Book/GetChapterContent
        public ActionResult GetChapterContent(BookViewModel viewModel, int chapterId)
        {
            var content = _dbContext.Chapters.FirstOrDefault(c => c.ChapterID == chapterId).Content;
            //var chapter = viewModel.Chapters.FirstOrDefault(c => c.ChapterID == viewModel.SelectedChapterId);

            if (content != null)
            {
                return Content(content);
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
