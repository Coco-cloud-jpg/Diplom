using HealthCheck.Models;
using HealthCheck.WINAPI;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Text;

namespace HealthCheck.Services
{
    internal class ScreenCapturer
    {
        private readonly HttpClientService _httpClientService;
        private readonly HttpClient _httpClient;

        internal ScreenCapturer()
        {
            _httpClientService = new HttpClientService();
            _httpClient = new HttpClient();
        }
        public void SendScreenshotWebAPI(Guid recorderId, string url = $"{Constants.WebApiURL}api/screen")
        {
            try
            {
                System.Diagnostics.Trace.WriteLine("Capturing start");
                Image img = CaptureScreen();
                //Bitmap resizedImage = new Bitmap(img, new Size((int)(img.Width / 1.5), (int)(img.Height / 1.5)));
                using var stream = new MemoryStream();
                img.Save(stream, ImageFormat.Jpeg);
                //resizedImage.Save(stream, ImageFormat.Jpeg);
                byte[] bytes = stream.ToArray();

                var json = JsonConvert.SerializeObject(new ScreenshotCreateModel { Base64 = Convert.ToBase64String(bytes), RecorderId = recorderId });

                System.Diagnostics.Trace.WriteLine("Sending to api start");
                _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }

        public void SendEntryWebAPI(Guid recorderId, uint seconds)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new EntranceCreateDTO { Seconds = seconds, RecorderId = recorderId });

                _httpClient.PostAsync($"{Constants.WebApiURL}api/entrance", new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }
        public void SendActivityWebAPI(Guid recorderId, double mouseActivity, double keyboardActivity)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new PheripheralActivity { 
                    MouseActivity = mouseActivity, 
                    KeyboardActivity = keyboardActivity,
                    RecorderId = recorderId });

                _httpClient.PostAsync($"{Constants.WebApiURL}api/pheripheral", new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }

        public void SendAppsUsageWebAPI(Guid recorderId, IEnumerable<AppFullInfo> appsInfo)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new AppInfoSTransferDTO
                {
                    AppsInfo = appsInfo,
                    RecorderId = recorderId
                });

                _httpClient.PostAsync($"{Constants.WebApiURL}api/apps", new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }
        public Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }
        private Image CaptureWindow(IntPtr handle)
        {
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            GDI32.SelectObject(hdcDest, hOld);
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            Image img = Image.FromHbitmap(hBitmap);
            GDI32.DeleteObject(hBitmap);

            return img;
        }
    }
}
