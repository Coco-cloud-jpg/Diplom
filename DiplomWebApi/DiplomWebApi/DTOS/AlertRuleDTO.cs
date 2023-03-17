using System.ComponentModel.DataAnnotations;

namespace RecordingService.DTOS
{
    public class AlertRuleReadDTO
    {
        public Guid Id { get; set; }
        public string SendToEmail { get; set; }
        public string SerializedWords { get; set; }
        public DateTime DateCreated { get; set; }
        public string ToRecorder { get; set; }
    }
    public class AlertRuleCreateDTO
    {
        public Guid? RecorderId { get; set; }
        [Required]
        public string SendToEmail { get; set; }
        [Required]
        public string SerializedWords { get; set; }
    }
}
