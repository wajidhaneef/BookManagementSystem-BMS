using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem_BMS.Models
{
    public class CoverPage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public string? ImageUrl { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }

}
