using ESMLoop.LoggingData;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace ESMLoop.Logging.SoftwareLoggers
{
    internal class ActiveWindowLogger : AbstractLeafLogger<ActiveWindowLoggingData>
    {
        const int SAMPLING_INTERVAL = 1000;

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "TitleActiveWindow",
            "WindowWidth",
            "WindowHeight",
            "WindowPosition_x",
            "WindowPosition_y"
        }.ToCSVString();

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]

        public struct RECT
        {
            public int Left;        // x position of upper-left corner  
            public int Top;         // y position of upper-left corner  
            public int Right;       // x position of lower-right corner  
            public int Bottom;      // y position of lower-right corner  
        }

        private readonly Timer _repeatSampling;
        private string _previousWindowTitle = String.Empty;

        public ActiveWindowLogger()
        {
            _repeatSampling = new Timer
            {
                AutoReset = true,
                Interval = SAMPLING_INTERVAL
            };
            _repeatSampling.Elapsed += Sample;
        }

        public override void Start(DateTime currentTime)
        {
            base.Start(currentTime);
            _repeatSampling.Start();
        }

        public override void Stop()
        {
            base.Stop();
            _repeatSampling.Stop();
        }

        private void Sample(Object? source, ElapsedEventArgs? e)
        {
            const int nChar = 256;
            StringBuilder stringBuilder = new(nChar);
            IntPtr activeWindow = GetForegroundWindow();
            if (GetWindowText(activeWindow, stringBuilder, nChar) == 0) return;
            GetWindowRect(activeWindow, out RECT rect);

            string currentWindow = stringBuilder.ToString().MakeAnonymous();

            if (_previousWindowTitle == currentWindow) return;
            _previousWindowTitle = currentWindow;

            LoggingData.Enqueue(new(
                titleActiveWindow: currentWindow,
                height: rect.Top - rect.Bottom,
                width: rect.Right - rect.Left,
                position: new int[2] { rect.Left, rect.Top }));

        }
    }
}
