using System.ComponentModel.DataAnnotations;

namespace Identity.DTO
{
    public class ResetPasswordRequest
    {
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordSubmit
    {
        [Required]
        public Guid RequestId { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
