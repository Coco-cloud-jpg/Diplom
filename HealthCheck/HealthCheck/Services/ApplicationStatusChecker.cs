using HealthCheck.Models;
using HealthCheck.WINAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Services
{
    internal class ApplicationStatusChecker
    {
        private bool _isStoped = true;
        private System.Windows.Forms.Timer? _timer;
        private string previosAppNamePath = null;
        private uint secondsSpent = 0;
        private Dictionary<string, AppInfoBase> _appsUsage = new Dictionary<string, AppInfoBase>();
        public Dictionary<string, AppInfoBase> AppsUsage { get => _appsUsage; }
        public ApplicationStatusChecker()
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(TimerEventProcessor);
            _isStoped = false;
        }
        public void Stop()
        {
            _isStoped = true;

            if (previosAppNamePath == null)
                return;

            if (_appsUsage.TryGetValue(Path.GetFileNameWithoutExtension(previosAppNamePath), out var prev))
            {
                prev.Seconds += secondsSpent;
                secondsSpent = 0;
            }
        }

        public void Start()
        {
            _isStoped = false;

            if (!_timer.Enabled)
                _timer.Start();
        }
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (_isStoped)
                return;

            IntPtr foregroundWindowHandle = IntPtr.Zero;
            int processId = 0;

            foregroundWindowHandle = User32.GetForegroundWindow();
            User32.GetWindowThreadProcessId(foregroundWindowHandle, out processId);

            var currentAppNamePath = GetProcessNameById(processId);

            if (currentAppNamePath != previosAppNamePath)
            {
                if (currentAppNamePath != null)
                {
                    var appName = Path.GetFileNameWithoutExtension(currentAppNamePath);

                    if (_appsUsage.ContainsKey(appName))
                        _appsUsage[appName].Seconds += secondsSpent;
                    else
                    {
                        try
                        {
                            Icon icon = Icon.ExtractAssociatedIcon(currentAppNamePath);
                            var bitMap = icon.ToBitmap();

                            using (var stream = new MemoryStream())
                            {
                                bitMap.Save(stream, ImageFormat.Png);
                                byte[] iconData = stream.ToArray();
                                string base64Icon = Convert.ToBase64String(iconData);

                                _appsUsage.Add(appName, new AppInfoBase() { IconBase64 = base64Icon, Seconds = secondsSpent });
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                secondsSpent = 0;
                previosAppNamePath = currentAppNamePath;
            }

            ++secondsSpent;
        }
        private string GetProcessNameById(int processId)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                return process?.MainModule?.FileName;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
