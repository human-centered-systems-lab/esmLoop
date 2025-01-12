using System;

namespace ESMLoop.Logging
{
    internal interface ILogger
    {
        /// <summary>
        ///     Start collecting data and keep it in List of Serialisable Classes
        /// </summary>
        internal void Start(DateTime currentTime);
        /// <summary>
        ///     Stop collecting
        /// </summary>
        internal void Stop();
        /// <summary>
        ///     Write current List to file
        /// </summary>
        internal void Log();
        /// <summary>
        ///     Inject save location for Logging-Data
        /// </summary>
        internal void SetSaveLocation(string path);
    }
}
