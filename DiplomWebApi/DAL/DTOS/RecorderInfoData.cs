using DiplomWebApi.DTOS;

namespace DAL.DTOS
{
    public class RecorderInfoData: GridResult<ScreenshotReadDTO>
    {
        public string HolderFullName { get; set; }
    }
}
