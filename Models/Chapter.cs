using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem_BMS.Models
{
    [Table("Chapter", Schema = "BMS")]
    public class Chapter
    {
        [Key]
        public int ChapterID { get; set; }
        [Required]public string ChapterName { get; set; }
        [Required]public string Content { get; set; }

        [Required]public int BookID { get; set; }
        public Book Book { get; set; }
    }
}
