using ESMLoop.Encryption;
using System;
using System.IO;
using System.Timers;

namespace ESMLoop.Logging
{
    internal class LoggingManager
    {
        private const string FILENAME = "esmLoop_FieldStudy";
        private const int LOGGING_INTERVAL = 10000;

        private readonly Logger _logger;
        private readonly Timer _repeatLogging;
        private string? _folderPath;

        internal bool IsRunning { get; private set; }
        internal bool IsReady
        {
            get
            {
                string path = Properties.Settings.Default.Path;
                return Directory.Exists(path) && !IsRunning;
            }
        }

        internal LoggingManager()
        {
            _repeatLogging = new Timer
            {
                AutoReset = true,
                Interval = LOGGING_INTERVAL
            };
            _repeatLogging.Elapsed += Log;

            _logger = new Logger();
        }


        internal void Start()
        {
            if (!IsReady || IsRunning) return;
            IsRunning = true;
            DateTime startTime = DateTime.Now;
            Properties.Settings.Default.StartTime = startTime;
            _folderPath = CreateDirectory(Properties.Settings.Default.Path, startTime);
            _logger.Start(startTime);
            _repeatLogging.Start();
        }

        internal void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            _repeatLogging.Stop();
            _logger.Stop();
            Encrypt();
        }

        internal void Encrypt()
        {
            if (_folderPath != null)
            {
                Encryptor.EncryptLoggingData(_folderPath);
                //Encryptor.DecryptLoggingData(_folderPath);
            }
        }
        internal void Log() => _logger.Log();

        internal void Log(Object? source, ElapsedEventArgs? e) => Log();

        internal string CreateDirectory(string path, DateTime currentTime)
        {
            string folderPath = Path.Combine(path, $"{FILENAME}_{currentTime:yyMMdd_HHmmss}");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            _logger.SetSaveLocation(folderPath);
            return folderPath;
        }
    }
}
