using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem_BMS.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public int RoleID { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        // Add more properties as needed

    }
}
