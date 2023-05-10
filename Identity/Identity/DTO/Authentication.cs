using System.ComponentModel.DataAnnotations;

namespace Identity.DTO
{
    public class AuthenticationRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class AuthenticationResponse
    {
        [Required]
        public string Access { get; set; }
        [Required]
        public string Refresh { get; set; }
        public long ValidUntil { get; set; }
        public string RedirectTo { get; set; }
    }
}
