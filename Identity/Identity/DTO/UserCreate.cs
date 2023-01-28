using System.ComponentModel.DataAnnotations;

namespace Identity.DTO
{
    public class UserCreate
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        public Guid RoleId { get; set; }
        public Guid CompanyId { get; set; }
    }
    public class AdminCreate
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
    }
}
