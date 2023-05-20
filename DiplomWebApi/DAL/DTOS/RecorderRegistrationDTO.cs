using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOS
{
    public class RecorderRegistrationReadDTO: BaseEntity
    {
        public DateTime TimeCreated { get; set; }
        public string HolderName { get; set; }
        public string HolderSurname { get; set; }
        public bool IsActive { get; set; }
        public int ScreenshotsTotal { get; set; }
        public int ScreenshotsToday { get; set; }
    }

    public class RecorderRegistrationCreateDTO
    {
        [Required]
        public string HolderName { get; set; }

        [Required]
        public string HolderSurname { get; set; }
    }
    public class RecorderRegistrationUpdateDTO: RecorderRegistrationCreateDTO
    {
    }
}
