using DiplomWebApi.DTOS;

namespace RecordingService.DTOS
{
    public class RecorderInfoData: GridResult<ScreenshotReadDTO>
    {
        public string HolderFullName { get; set; }
    }
}
