using ESMLoop.LoggingData;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ESMLoop.Logging.SoftwareLoggers
{
    internal class SoftwareInteractionLogger : AbstractLeafLogger<SoftwareInteractionLoggingData>
    {
        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "Line",
            "ClassName",
            "MethodName",
            "Message"

        }.ToCSVString();

        public override void Log()
        {
            if (Debug.IsEmpty) return;
            foreach (var log in Debug.GetLogs())
            {
                LoggingData.Enqueue(log);
            }
            base.Log();
        }
    }

    internal static class Debug
    {
        private static readonly List<SoftwareInteractionLoggingData> _logs = new();

        public static bool IsEmpty
        {
            get
            {
                return _logs.Count == 0;
            }
        }

        internal static void Log(string message, [CallerFilePath] string path = "Error", [CallerLineNumber] int number = 0, [CallerMemberName] string name = "Error")
        {
            _logs.Add(new(message, name, path, number));
        }

        internal static List<SoftwareInteractionLoggingData> GetLogs()
        {
            List<SoftwareInteractionLoggingData> logs = new(_logs);
            _logs.Clear();
            return logs;
        }
    }

}
