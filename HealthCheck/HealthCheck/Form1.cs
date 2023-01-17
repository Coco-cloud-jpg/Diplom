using HealthCheck.Services;
using HealthCheck.WINAPI;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;

namespace HealthCheck
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer? _myTimer;
        private const string _onText = "START";
        private const string _offText = "STOP";
        private const int _msInSecond = 1000;
        private readonly string _currentMacAddress;
        private Keyboard keyboardhook;
        private StringBuilder currentWord = new StringBuilder();
        private long _lastInputTimeStamp = 0;
        //private readonly string _currentMachineName;
        private readonly ScreenCapturer _screenCapturer;
        public Form1()
        {
            this.TopMost = true;
            this.Focus();
            this.TopMost = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //User32.AllowSetForegroundWindow((uint)Process.GetCurrentProcess().Id);
            //User32.SetForegroundWindow(Handle);
            //User32.ShowWindow(Handle, User32.SW_SHOWNORMAL);

            InitializeComponent();

            //_currentMachineName = Environment.MachineName;
            _currentMacAddress = GetMACAddress();
            _screenCapturer = new ScreenCapturer();
            if (!CheckAccess(_currentMacAddress, Guid.NewGuid()))
            {
                buttonWrapper.Hide();
                activityToggle.Hide();
                alertField.Text = "This recorder isn't registered in system.\nPlease contact support.";
            }
            //// true to suppress key press = textBox1_KeyUp will not be called if event KeyUp event is handled
            //this.keyboardhook = new Keyboard(true);

            //// add keys to listen or set HookAllKeys to true to listen all

            //// set event handler and hook
            //this.keyboardhook.KeyUp += new KeyEventHandler(keyboardhook_KeyUp);
            //this.keyboardhook.Hook();
        }
        // handle the keyboard hook event
        void keyboardhook_KeyUp(object sender, KeyEventArgs e)
        {
            Console.ReadKey();
            if (!_myTimer.Enabled)
                return;
            // just write key code and mark as handled

            if (_lastInputTimeStamp < DateTime.UtcNow.Ticks - 10000*1000*5)//10000 ticks in ms (e.KeyCode == Keys.Space)
            {
                Task.Run(() =>
                {
                    //var res = TypeDescriptor.GetConverter(InputLanguage.CurrentInputLanguage.Culture).ConvertFrom();
                    File.AppendAllText("log.txt", $"{currentWord.ToString()} - {DateTime.UtcNow}\n");
                    currentWord.Clear();
                });
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (currentWord.Length > 0)
                    currentWord.Remove(currentWord.Length - 1, 1);
            }
            else
            {
                var toAppend = e.KeyCode.ToString();
                var isUpperCase = (((ushort)Keyboard.GetKeyState(0x14)) & 0xffff) != 0;
                KeysConverter kc = new KeysConverter();

                if (e.KeyCode == Keys.Space)
                    toAppend = " ";

                toAppend = isUpperCase? toAppend.ToUpper(): toAppend.ToLower();
               
                currentWord.Append(toAppend);
            }
            _lastInputTimeStamp = DateTime.UtcNow.Ticks;
            e.Handled = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // dispose keyboard hook
            //this.keyboardhook.KeyUp -= keyboardhook_KeyUp;
            //this.keyboardhook.Dispose();
        }
        private void activityToggle_Click(object sender, EventArgs e)
        {
            if (_myTimer == null)
            {
                TimerConfig();
                activityToggle.Text = _offText;
                activityToggle.BackColor = Color.Red;

                Task.Run(async () =>
                {
                    var rnd = new Random();
                    while (true)
                    {
                        if (_myTimer.Enabled)
                        {
                            _screenCapturer.SendToWebAPI(_currentMacAddress);
                            await Task.Delay(_msInSecond * 60 * 3 + rnd.Next(0,180) * _msInSecond);//5 minutes delay when screenshot craeted
                        }

                        await Task.Delay(_msInSecond * 5);//5 second delay to next iteration
                    }
                });
            }
            else
            {
                ToggleTimer();
            }
        }
        private void ToggleTimer()
        {
            if (_myTimer.Enabled)
            {
                activityToggle.Text = _onText;
                activityToggle.BackColor = Color.Green;
                _myTimer.Stop();
            }
            else
            {
                activityToggle.Text = _offText;
                activityToggle.BackColor = Color.Red;
                _myTimer.Start();
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
            _myTimer.Start();
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
        private class ScreenAuthorizationDTO
        {
            public Guid CustomerId { get; set; }
            public string MachineKey { get; set; }
        }
        private bool CheckAccess(string macAddress, Guid customerId)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44375/api/screen/authorize");
                request.Content = new StringContent(JsonConvert.SerializeObject(new ScreenAuthorizationDTO { MachineKey = macAddress, CustomerId = customerId }));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);
                var responseRaw = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);

                return responseRaw;
            }
        }
        private static string AddressBytesToString(byte[] addressBytes)
        {
            return string.Join("-", (from b in addressBytes
                                     select b.ToString("X2")).ToArray());
        }
    }
}