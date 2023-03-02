using System.ComponentModel.DataAnnotations;

namespace RecordingService.DTOS
{
    public class RecorderRegistrationDTO
    {
        [Required]
        public string HolderName { get; set; }

        [Required]
        public string HolderSurname { get; set; }
    }
}
