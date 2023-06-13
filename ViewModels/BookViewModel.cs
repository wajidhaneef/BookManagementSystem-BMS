using BookManagementSystem_BMS.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BookManagementSystem_BMS.ViewModels
{
    public class BookViewModel
    {
        public List<Category> AllCategories { get; set; }
        public List<Book> Books { get; set; }
        public List<Chapter> Chapters { get; set; }
        public string SelectedBook { get; set; }
        public string SelectedCategory { get; set; }
        public int SelectedCategoryId { get; set; }
        public int SelectedBookId { get; set; }
        public int SelectedChapterId { get; set; }
        public string SelectedChapter { get; set; }
        public string SelectedChapterContent { get; set; }
        public List<CoverPageViewModel> CoverPages { get; set; }
        public string LoggedIn { get; set;}

        [Required(ErrorMessage = "Role is required")]
        public List<Role> Roles { get; set; }
        public int SelectedRoleId { get; set; }
    }
}
