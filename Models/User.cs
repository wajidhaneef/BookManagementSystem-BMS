using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace BookManagementSystem_BMS.Models
{
    [Table("User", Schema = "BMS")]
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]public string Username { get; set; }
        [EmailAddress]
        [Required] public string EmailAddress { get; set; }
        [Required]public string PasswordHash { get; set; }
        [Required]public string Salt { get; set; }
        [Required]public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}
