using BookManagementSystem_BMS.Models;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem_BMS.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Must be between 3 and 50 characters.", MinimumLength = 3)]
        [Display(Name = "Full Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        public List<Role> Roles { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int SelectedRoleId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(18, ErrorMessage = "Must be between 8 and 18 characters.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "The Password must contain a small letter, a capital letter, a number and a special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Confirm Password is required")]
        //[StringLength(18, ErrorMessage = "Must be between 8and 18 characters", MinimumLength = 8)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}
