using ESMLoop.Logging.Loggers;

namespace ESMLoop.Logging
{
    internal class Logger : AbstractCompositLogger
    {
        internal Logger()
        {
            _loggers.Add(new InteractionLibraryLogger());
            _loggers.Add(new SoftwareLogger());
            _loggers.Add(new QuestionLogger());
            _loggers.Add(new TobiiProSDKLogger());
        }
    }
}
