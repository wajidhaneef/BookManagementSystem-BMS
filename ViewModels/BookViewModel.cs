using BookManagementSystem_BMS.Models;

namespace BookManagementSystem_BMS.ViewModels
{
    public class BookViewModel
    {
        public List<Category> AllCategories { get; set; }
        public string SelectedCategory { get; set; }
        public int SelectedCategoryId { get; set; }
        public List<Book> BooksByCategory { get; set; }
        public int SelectedBookId { get; set; }
        public List<Chapter> ChaptersByBook { get; set; }
        public int SelectedChapterId { get; set; }
        public string SelectedChapterContent { get; set; }
    }
}
