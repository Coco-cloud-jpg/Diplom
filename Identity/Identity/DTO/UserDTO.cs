using System.ComponentModel.DataAnnotations;

namespace Identity.DTO
{
    public class UserCreateDTO
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

    public class UserReadDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
    public class UserUpdateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid Role { get; set; }
    }

    public class SubmitUserRegisration
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
