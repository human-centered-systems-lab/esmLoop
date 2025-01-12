using ESMLoop.LoggingData;
using ESMLoop.Properties;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace ESMLoop.Logging
{
    internal abstract class AbstractLeafLogger<LoggingDataType> : ILogger where LoggingDataType : AbstractLoggingData
    {
        #region PathInjection
        public const string FILE_ENDING = "TEMP";
        protected abstract string CSV_Header { get; }

        protected string? _filePath;
        protected string? _directoryPath;

        public void SetSaveLocation(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();
            _directoryPath = path;

        }
        protected void CreateFilePath(DateTime startTime)
        {
            if (!Directory.Exists(_directoryPath)) throw new DirectoryNotFoundException();
            string fileName = $"{typeof(LoggingDataType).Name}_{startTime:yyMMdd_HHmmss}_{Settings.Default.UserID}.{FILE_ENDING}";
            _filePath = Path.Combine(_directoryPath, fileName);
        }

        protected void CreateFile()
        {
            if (_filePath == null) throw new DirectoryNotFoundException();
            File.WriteAllText(_filePath, CSV_Header + Environment.NewLine);
        }
        #endregion

        #region Logging
        protected ConcurrentQueue<LoggingDataType> LoggingData = new();

        protected static object lockObj = new();

        public virtual void Log()
        {
            lock (lockObj)
            {
                int count = LoggingData.Count;
                if (_filePath == null || count == 0) return;

                StringBuilder sb = new();

                do
                {
                    LoggingData.TryDequeue(out LoggingDataType? data);
                    if (data == null) continue;
                    sb.Append(data.ToCSVString() + Environment.NewLine);
                    count--;
                } while (count > 0);

                while (true)
                {
                    try
                    {
                        File.AppendAllText(_filePath, sb.ToString());
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    break;
                }
            }
        }
        #endregion

        #region Start Stop
        public virtual void Start(DateTime currentTime)
        {
            CreateFilePath(currentTime);
            CreateFile();
        }

        public virtual void Stop()
        {
            Log();
        }
        #endregion
    }
}
