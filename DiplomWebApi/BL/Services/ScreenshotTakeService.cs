using BL.Services.Interfaces;
using Common.Models;
using Common.Services.Interfaces;
using DAL.DTOS;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace BL.Services
{
    public class ScreenshotTakeService: IScreenshotTakeService
    {
        private IScreenUnitOfWork _unitOfWork;
        private IOcrService _ocrService;
        private IBlobService _blobService;
        public ScreenshotTakeService(IScreenUnitOfWork unitOfWork, IBlobService blobService, IOcrService ocrService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
            _ocrService = ocrService;
        }
        //TODO Add recorder token login table, though only one instance can add screenshots via login
        public async Task AddScreenShot(ScreenshotCreateDTO model)
        {
            var companyId = await _unitOfWork.RecorderRegistrationRepository.DbSet
                        .Where(item => item.Id == model.RecorderId && item.IsActive)
                        .Select(item => item.CompanyId).FirstOrDefaultAsync();

            if (companyId == null)
                throw new ArgumentNullException();

            var load = Convert.FromBase64String(model.Base64);
            var img = Image.Load(load);

            img.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(img.Width / 2, img.Height / 2)
            }));

            var dateTimeNow = DateTime.UtcNow;

            var screenId = Guid.NewGuid();
            var path = $"{companyId}/{model.RecorderId}/{screenId}.jpeg";
            var base64Resized = img.ToBase64String(JpegFormat.Instance).Split(",")[1];
            await _blobService.UploadFileBlobAsync("screenshots", base64Resized, path);

            await _unitOfWork.ScreenshotRepository.Create(new Screenshot
            {
                Id = screenId,
                RecorderId = model.RecorderId,
                DateCreated = dateTimeNow,
                StorePath = path,
            }, CancellationToken.None);

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            //not awaited proccessing
            await _ocrService.Process(model.Base64, companyId, model.RecorderId, screenId, _unitOfWork);
        }
    }
}
