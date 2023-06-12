using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BookManagementSystem_BMS.Models;

namespace BookManagementSystem_BMS.Data
{
    public static class DummyDataGenerator
    {
        public static List<Category> GenerateCategories()
        {
            var categories = new List<Category>
            {
                new Category { CategoryID = 1, CategoryName = "Fiction" },
                new Category { CategoryID = 2, CategoryName = "Non-Fiction" },
                new Category { CategoryID = 3, CategoryName = "Mystery" },
                // Add more categories as needed
            };

            return categories;
        }

        public static List<Book> GenerateBooks()
        {
            var books = new List<Book>
            {
                new Book { BookID = 1, BookName = "Book 1", CategoryID = 1 },
                new Book { BookID = 2, BookName = "Book 2", CategoryID = 2 },
                new Book { BookID = 3, BookName = "Book 3", CategoryID = 3 },
                // Add more books as needed
            };

            return books;
        }

        public static List<Chapter> GenerateChapters()
        {
            var chapters = new List<Chapter>
            {
                new Chapter { ChapterID = 1, ChapterName = "Chapter 1 B1", BookID = 1, Content = "Chapter 1 content..."  },
                new Chapter { ChapterID = 2, ChapterName = "Chapter 2 B1", BookID = 1, Content = "Chapter 2 content..."  },
                new Chapter { ChapterID = 3, ChapterName = "Chapter 1 B2", BookID = 2, Content = "Chapter 2 content..."  },
                new Chapter { ChapterID = 4, ChapterName = "Chapter 2 B2", BookID = 2, Content = "Chapter 2 content..."  },
                new Chapter { ChapterID = 5, ChapterName = "Chapter 1 B3", BookID = 3, Content = "Chapter 2 content..."  },
                new Chapter { ChapterID = 6, ChapterName = "Chapter 2 B3", BookID = 3, Content = "Chapter 2 content..."  },
            };

            return chapters;
        }

        public static List<CoverPage> GenerateCoverPages()
        {
            var coverPages = new List<CoverPage>
            {
                new CoverPage { Id = 1, BookId = 1, ImageData = GetImageData("cover1.jpg") },
                new CoverPage { Id = 2, BookId = 2, ImageData = GetImageData("cover2.jpg") },
                new CoverPage { Id = 3, BookId = 3, ImageData = GetImageData("cover3.jpg") },
                // Add more cover pages as needed
            };

            return coverPages;
        }

        private static byte[] GetImageData(string imagePath)
        {
            // Read the image file and convert it to binary data
            var imageData = File.ReadAllBytes(imagePath);
            return imageData;
        }

        //public static void Main(string[] args)
        //{
        //    var categories = GenerateCategories();
        //    var books = GenerateBooks(categories);
        //    var chapters = GenerateChapters(books);
        //    var coverPages = GenerateCoverPages(books);

        //    // Perform any necessary operations with the generated dummy data

        //    Console.WriteLine("Dummy data generation complete.");
        //    Console.ReadLine();
        //}
    }
}
