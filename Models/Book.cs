using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem_BMS.Models
{
    [Table("Book", Schema = "BMS")]
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        [Required] public string BookName { get; set; }

        [Required]public int CategoryID { get; set; }
        [Required]public Category Category { get; set; }

        [Required]public List<Chapter> Chapters { get; set; }
    }
}
