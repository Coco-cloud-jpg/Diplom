using Common.Emails;
using Common.Services.Interfaces;
using IronOcr;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using BL.Services.Interfaces;
using System.Text.RegularExpressions;
using DAL.Interfaces;

namespace BL.Services
{
    public class OcrService: IOcrService
    {
        private readonly IEmailSender _emailSender;
        public readonly IronTesseract _engine;
        public OcrService(IEmailSender emailSender)
        {
            _engine = new IronTesseract();
            _emailSender = emailSender;
        }

        public async Task Process(string base64, Guid companyId, Guid recorderId, Guid screenshotId, IScreenUnitOfWork _unitOfWork)
        {
            try
            {
                var screenshot = _unitOfWork.ScreenshotRepository.DbSet.FirstOrDefault(screenshot => screenshot.Id == screenshotId);

                var rules = _unitOfWork.AlertRuleRepository.DbSet.AsNoTracking()
                .Where(item => item.CompanyId == companyId).ToList().Where(item => item.RecorderId == null ? true : item.RecorderId == recorderId);

                if (rules.Count() == 0 || screenshot == null)
                    return;

                using (var Input = new OcrInput(Convert.FromBase64String(base64)))
                {
                    var result = _engine.Read(Input).Text.ToLower();

                    var template = System.IO.File.ReadAllText("Templates/AlertRuleTemplate.html");

                    foreach (var item in rules)
                    {
                        foreach (var word in JsonConvert.DeserializeObject<List<string>>(item.SerializedWords))
                        {
                            string pattern = $@"\b{word.ToLower()}\b";
                            Regex regex = new Regex(pattern);

                            if (regex.IsMatch(result.ToLower()))
                            {
                                var concreteTemplate = template.Replace("{{word}}", word)
                                                   .Replace("{{screenshotId}}", screenshotId.ToString())
                                                   .Replace("{{recorderId}}", screenshot.RecorderId.ToString());

                                var message = new Message(new string[] { item.SendToEmail }, "Word occurence!", concreteTemplate);

                                _emailSender.SendEmail(message);

                                if (screenshot.Mark != Common.Models.AlertState.InternalWarning)
                                {
                                    screenshot.Mark = Common.Models.AlertState.InternalWarning;
                                    await _unitOfWork.SaveChangesAsync(CancellationToken.None);
                                }

                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var s = 2;
            }
            
        }
    }
}
