using HealthCheck.Models;
using HealthCheck.Services;
using HealthCheck.WINAPI;
using Newtonsoft.Json;
using System.Management;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;

namespace HealthCheck
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer? _myTimer;
        private const string _onText = "START";
        private bool _authorized = false;
        private const string _offText = "STOP";
        private const int _msInSecond = 1000;
        private readonly string _currentMacAddress;
        private Guid _companyId = Guid.Parse("8414B35A-1592-40A6-BFCA-EE89DC36D05B");
        private Guid _recorderId = Guid.Parse("E2B7C12C-CDA8-4FDA-B6F0-CE5450679F2B");
        private Keyboard keyboardhook;
        private StringBuilder currentWord = new StringBuilder();
        private long _lastInputTimeStamp = 0;
        private IPheripheralController _mouseMonitor;
        private IPheripheralController _keyboardMonitor;
        private ApplicationStatusChecker _applicationStatusChecker;
        //private readonly string _currentMachineName;
        private readonly ScreenCapturer _screenCapturer;
        private PrivateMessageHub _privateMessageHub;
        public Form1()
        {
            this.TopMost = true;
            this.Focus();
            this.TopMost = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //string hostName = Dns.GetHostName();
            //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //User32.AllowSetForegroundWindow((uint)Process.GetCurrentProcess().Id);
            //User32.SetForegroundWindow(Handle);
            //User32.ShowWindow(Handle, User32.SW_SHOWNORMAL);

            InitializeComponent();

            //_currentMachineName = Environment.MachineName;
            //_currentMacAddress = GetMACAddress();
            _screenCapturer = new ScreenCapturer();
            _authorized = CheckAccess(_recorderId, _companyId);
            if (!_authorized)
            {
                buttonWrapper.Hide();
                activityToggle.Hide();
                alertField.Text = "This recorder isn't registered in system.\nPlease contact support.";
            }
            _mouseMonitor = new MouseStatusChecker();
            _keyboardMonitor = new KeyBoardStatusChecker();
            _applicationStatusChecker = new ApplicationStatusChecker();
        }

        private void activityToggle_Click(object sender, EventArgs e)
        {
            if (_myTimer == null)
            {
                _privateMessageHub = new PrivateMessageHub(_recorderId);
                _privateMessageHub.Start();
                TimerConfig();
                Task.Run(async () =>
                {
                    var rnd = new Random();
                    while (true)
                    {
                        if (_myTimer.Enabled)
                        {
                            _screenCapturer.SendScreenshotWebAPI(_recorderId);
                            await Task.Delay(_msInSecond * 60 * 3 + rnd.Next(0, 180) * _msInSecond);//5 minutes delay when screenshot craeted
                        }

                        await Task.Delay(_msInSecond * 5);//5 second delay to next iteration
                    }
                });
            }

            ToggleTimer();
        }
        private void ToggleTimer()
        {
            if (_myTimer.Enabled)
            {
                activityToggle.Text = _onText;
                activityToggle.BackColor = Color.Green;
                _myTimer.Stop();
                _mouseMonitor.Stop();
                _keyboardMonitor.Stop();
                _applicationStatusChecker.Stop();
            }
            else
            {
                activityToggle.Text = _offText;
                activityToggle.BackColor = Color.Red;
                _myTimer.Start();
                _mouseMonitor.Start();
                _keyboardMonitor.Start();
                _applicationStatusChecker.Start();
            }
        }
        private void TimerConfig()
        {
            _myTimer = new System.Windows.Forms.Timer();
            _myTimer.Interval = _msInSecond;
            _myTimer.Tick += new EventHandler((sender, e) =>
            {
                var components = timer.Text.Split(":");

                var seconds = Convert.ToInt16(components[2]);
                var minutes = Convert.ToInt16(components[1]);
                var hours = Convert.ToInt16(components[0]);

                ++seconds;

                if (seconds == 60)
                {
                    seconds = 0;
                    ++minutes;
                }

                if (minutes == 60)
                {
                    minutes = 0;
                    ++hours;
                }

                timer.Text = $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
            });
        }

        private static string GetMACAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                    return AddressBytesToString(nic.GetPhysicalAddress().GetAddressBytes());
            }

            return string.Empty;
        }   
        private bool CheckAccess(Guid recorderId, Guid companyId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, $"{Constants.WebApiURL}api/recordings/authorize");
                    request.Content = new StringContent(JsonConvert.SerializeObject(new ScreenAuthorizationDTO { RecorderId = recorderId, CompanyId = companyId }));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);

                    if (response.IsSuccessStatusCode)
                        _recorderId = JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().Result);

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
        private static string AddressBytesToString(byte[] addressBytes)
        {
            return string.Join(":", (from b in addressBytes
                                     select b.ToString("X2")).ToArray());
        }

        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            if (_authorized && _myTimer != null)
            {
                var components = timer.Text.Split(":");

                var seconds = Convert.ToInt16(components[2]);
                var minutes = Convert.ToInt16(components[1]);
                var hours = Convert.ToInt16(components[0]);

                _screenCapturer.SendEntryWebAPI(_recorderId, (uint)(seconds + minutes * 60 + hours * 3600));

                var mouseActivity = _mouseMonitor.GetWorkPercentage();
                var keyboardActivity = _keyboardMonitor.GetWorkPercentage();

                _screenCapturer.SendActivityWebAPI(_recorderId, mouseActivity, keyboardActivity);
                _applicationStatusChecker.Stop();
                var appsUsages = _applicationStatusChecker.AppsUsage.ToList().Select(item => new AppFullInfo
                {
                    Name = item.Key,
                    IconBase64 = item.Value.IconBase64,
                    Seconds = item.Value.Seconds
                }).ToList();


                _screenCapturer.SendAppsUsageWebAPI(_recorderId, appsUsages);
            }
        }
    }
}