using Common.Models;

namespace RecordingService.DTOS
{
    public class ScreenshotWarning
    {
        public string Base64 { get; set; }
        public string Path { get; set; }
        public Guid Id { get; set; }
    }
    public class WarningCommentAddDTO
    {
        public string Text { get; set; }
        public short Mark { get; set; }
        public bool PostComment { get; set; }
    }
}
