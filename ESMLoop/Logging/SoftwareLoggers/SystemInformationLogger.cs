using ESMLoop.LoggingData;
using System;

namespace ESMLoop.Logging.SoftwareLoggers
{
    internal class SystemInformationLogger : AbstractLeafLogger<SystemInformationLoggingData>
    {
        protected override string CSV_Header => new string[]
        {
            "Width",
            "Height",
            "Name"
        }.ToCSVString();

        public override void Start(DateTime currentTime)
        {
            base.Start(currentTime);
            //var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                LoggingData.Enqueue(new(
                    screenName: screen.DeviceName,
                    screenWidth: screen.Bounds.Width,
                    screenHeight: screen.Bounds.Height
                    ));
            }
        }
        //Stop Logging?
    }
}
