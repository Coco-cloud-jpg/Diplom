using HealthCheck.WINAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HealthCheck.Services
{
    public class MouseStatusChecker: IPheripheralController
    {
        private Point previousMousePosition;
        private Stopwatch _totalStopwatch;
        private Stopwatch _dragginStopwatch;
        private bool _isStoped = true;
        private System.Windows.Forms.Timer? _timer;
        public MouseStatusChecker()
        {
            _totalStopwatch = new Stopwatch();
            _dragginStopwatch = new Stopwatch();
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
            _dragginStopwatch.Stop();
        }

        public void Start()
        {
            _isStoped = false;
            _totalStopwatch.Start();
            _dragginStopwatch.Start();
        }
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (_isStoped)
                return;

            Point currentMousePosition = Cursor.Position;

            if (Math.Abs(currentMousePosition.X - previousMousePosition.X) < 10 ||
                Math.Abs(currentMousePosition.Y - previousMousePosition.Y) < 10)
                _dragginStopwatch.Stop();
            else if (!_dragginStopwatch.IsRunning)
                _dragginStopwatch.Start();

            previousMousePosition = currentMousePosition;
        }

        public double GetWorkPercentage()
        {
            _timer.Stop();
            _totalStopwatch.Stop();
            _dragginStopwatch.Stop();
            return _dragginStopwatch.ElapsedMilliseconds / (double)_totalStopwatch.ElapsedMilliseconds;
        }
    }
}
