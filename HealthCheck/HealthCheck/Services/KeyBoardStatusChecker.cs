using HealthCheck.WINAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Services
{
    public class KeyBoardStatusChecker : IPheripheralController
    {
        private Stopwatch _totalStopwatch;
        private Stopwatch _typingStopwatch;
        private System.Windows.Forms.Timer? _timer;
        private bool _isStoped = true;
        private Keyboard keyboardhook;
        private DateTime _lastInputTimeStamp = DateTime.UtcNow;
        public KeyBoardStatusChecker()
        {
            _totalStopwatch = new Stopwatch();
            _typingStopwatch = new Stopwatch();
            keyboardhook = new Keyboard(true);
            keyboardhook.KeyDown += new KeyEventHandler(KeyboardHook);
            keyboardhook.Hook();
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(TimerEventProcessor);
            _timer.Start();
            _isStoped = true;
        }
        public void Stop()
        {
            _isStoped = true;
            _totalStopwatch.Stop();
            _typingStopwatch.Stop();
        }

        public void Start()
        {
            _isStoped = false;
            _totalStopwatch.Start();
            _typingStopwatch.Start();
        }
        void KeyboardHook(object sender, KeyEventArgs e)
        {
            if (_isStoped)
                return;

            _lastInputTimeStamp = DateTime.UtcNow;
            e.Handled = true;
        }

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (_isStoped)
                return;

            if ((DateTime.UtcNow - _lastInputTimeStamp).Seconds > 3)//10000 ticks in ms (e.KeyCode == Keys.Space)
                _typingStopwatch.Stop();
            else if (!_typingStopwatch.IsRunning)
                _typingStopwatch.Start();
        }

        public double GetWorkPercentage()
        {
            this.keyboardhook.KeyDown -= KeyboardHook;
            this.keyboardhook.Dispose();
            _totalStopwatch.Stop();
            _typingStopwatch.Stop();
            return _typingStopwatch.ElapsedMilliseconds / (double)_totalStopwatch.ElapsedMilliseconds;
        }
    }
}
