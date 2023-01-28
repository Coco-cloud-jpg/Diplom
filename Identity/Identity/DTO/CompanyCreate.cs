using System.ComponentModel.DataAnnotations;

namespace Identity.DTO
{
    public class CompanyCreate
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
