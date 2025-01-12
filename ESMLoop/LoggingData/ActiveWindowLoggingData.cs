using System;

namespace ESMLoop.LoggingData
{
    internal class ActiveWindowLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;

        public string TitleActiveWindow;

        public int WindowWidth;

        public int WindowHeight;

        public int[] WindowPosition;

        public ActiveWindowLoggingData(string titleActiveWindow, int width, int height, int[] position)
        {
            SystemTime = DateTime.Now;
            TitleActiveWindow = titleActiveWindow;
            WindowWidth = width;
            WindowHeight = height;
            WindowPosition = position;
        }

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                TitleActiveWindow.ToString(),
                WindowWidth.ToString(),
                WindowHeight.ToString(),
                WindowPosition.ToCSVString()
            };
            return content.ToCSVString();
        }
    }
}
