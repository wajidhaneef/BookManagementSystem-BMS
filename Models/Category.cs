using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem_BMS.Models
{
    [Table("Category", Schema = "BMS")]
    public class Category
    {
        [Key]public int CategoryID { get; set; }
        [Required]public string CategoryName { get; set; }
        public List<Role> Roles { get; set; }
        public List<Book> Books { get; set; }
    }
}
