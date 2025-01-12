using System;
using System.IO;
using System.Linq;

namespace ESMLoop.LoggingData
{
    internal class SoftwareInteractionLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;
        public int Line;
        public string Class;
        public string Method;
        public string Messsage;

        public SoftwareInteractionLoggingData(string messsage, string callername, string path, int line)
        {
            Class = path == null ? "Error" : path.Split(Path.DirectorySeparatorChar).Last();
            SystemTime = DateTime.Now;
            Messsage = messsage;
            Line = line;
            Method = callername;
        }

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                Line.ToString(),
                Class,
                Method,
                Messsage
            };
            return content.ToCSVString();
        }
    }
}
