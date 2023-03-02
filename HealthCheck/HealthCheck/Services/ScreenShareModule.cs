using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Services
{
    public class ScreenShareModule
    {
        private readonly ScreenCapturer _screenCapturer;
        private System.Windows.Forms.Timer? _timer;
        private readonly Guid _recorderId;
        private readonly Guid _userId;
        public ScreenShareModule(Guid recorderId, Guid userId)
        {
            _screenCapturer = new ScreenCapturer();
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 40;
            _timer.Tick += new EventHandler(TimerEventProcessor);
            _timer.Start();
            _recorderId = recorderId;
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            _screenCapturer.SendScreenshotWebAPI(_recorderId, $"{Constants.WebApiURL}api/screenshare/{_recorderId}/{_userId}");
        }
    }
}
