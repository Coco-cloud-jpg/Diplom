namespace DiplomWebApi.DTOS
{
    public class ScreenshotCreateDTO
    {
        public string Base64 { get; set; }
        public Guid RecorderId { get; set; }

    }
}
