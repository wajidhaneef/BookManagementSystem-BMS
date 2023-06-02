using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace BookManagementSystem_BMS.Models
{
    [Table("Role", Schema = "BMS")]
    public class Role
    {
        [Key]public int RoleID { get; set; }
        [Required]public string RoleName { get; set; }

        public List<User> Users { get; set; }
        public List<Category> Categories { get; set; }
    }
}
