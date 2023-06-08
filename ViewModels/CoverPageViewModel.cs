using System.Drawing;

namespace BookManagementSystem_BMS.ViewModels
{
    public class CoverPageViewModel
    {
        public Image CoverImage { get; set; }
        public string BookName { get; set; }
        public int BookId { get; set; }
        public int CategoryId { get; set; }
        public byte[] ImageContent { get; set; }
    }
}
