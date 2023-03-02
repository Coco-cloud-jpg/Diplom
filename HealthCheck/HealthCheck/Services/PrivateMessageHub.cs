
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace HealthCheck.Services
{
    public class ConnectModel
    {
        public string Caller { get; set; }
        public Guid RecorderId { get; set; }
    }

    public class ScreenMessage
    {
        public Guid RecorderId { get; set; }
        public string Base64 { get; set; }
    }
    internal class PrivateMessageHub
    {
        private readonly ScreenCapturer _screenCapturer;
        private readonly HubConnection _connection;
        private System.Windows.Forms.Timer? _timer;
        private string _caller = null;
        private bool toWork = true;
        public PrivateMessageHub(Guid recorderId)
        {
            _screenCapturer = new ScreenCapturer();
            _connection = new HubConnectionBuilder()
            .WithUrl($"{Constants.WebApiURL}screenShare")
            .Build();
            var id = _connection.ConnectionId;
            _connection.On<ConnectModel>("ReceiveMessage", async (message) =>
            {
                try
                {
                    Debug.WriteLine("asd");
                    Debug.WriteLine(message);

                    if (message.RecorderId == recorderId)
                    {
                        //TimerConfig();
                        toWork = true;
                        StartWork(recorderId);
                    }
                }
                catch (Exception e)
                {
                    Stop();
                    var s = 2;
                }
            });

            _connection.On<string>("StopSharing", (message) =>
            {
                if (message == recorderId.ToString())
                {
                    Stop();
                }
            });

            _connection.Closed += async (e) =>
            {
                Stop();
                await Start();
            };
        }
        public void Stop()
        {
            toWork = false;
        }
        private async Task StartWork(Guid recorderId)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (!toWork)
                            return;

                        Image img = _screenCapturer.CaptureScreen();
                        Bitmap resizedImage = new Bitmap(img, new Size(img.Width / 2, img.Height / 2));
                        using var stream = new MemoryStream();
                        resizedImage.Save(stream, ImageFormat.Jpeg);
                        byte[] bytes = stream.ToArray();

                        var model = new ScreenMessage { Base64 = Convert.ToBase64String(bytes), RecorderId = recorderId };

                        await _connection.InvokeAsync("SendScreenToCaller", model);
                        await Task.Delay(20);
                    }
                    catch (Exception e)
                    {
                        var s = 2;
                        Stop();

                        return;
                    }
                }
            });
        }
        public async Task Start()
        {
            while (_connection.State != HubConnectionState.Connected)
            {
                try
                {
                    await _connection.StartAsync();
                }
                catch (Exception e)
                {
                    var s = 2;
                }
                finally
                {
                    await Task.Delay(5000);
                }
            }
        }
        
    }
}
