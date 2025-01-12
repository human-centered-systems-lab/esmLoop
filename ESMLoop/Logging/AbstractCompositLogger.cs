using System;
using System.Collections.Generic;

namespace ESMLoop.Logging
{
    internal abstract class AbstractCompositLogger : ILogger
    {
        protected readonly List<ILogger> _loggers = new();

        public virtual void Start(DateTime currentTime) => _loggers.ForEach(logger => logger.Start(currentTime));

        public virtual void Stop() => _loggers.ForEach(logger => logger.Stop());

        public void Log() => _loggers.ForEach(logger => logger.Log());

        public void SetSaveLocation(string path) => _loggers.ForEach(logger => logger.SetSaveLocation(path));
    }
}
