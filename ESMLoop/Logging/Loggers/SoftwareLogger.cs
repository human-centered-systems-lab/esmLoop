using ESMLoop.Logging.SoftwareLoggers;


namespace ESMLoop.Logging.Loggers
{
    internal class SoftwareLogger : AbstractCompositLogger
    {
        public SoftwareLogger()
        {
            _loggers.Add(new ActiveWindowLogger());
            _loggers.Add(new SoftwareInteractionLogger());
            _loggers.Add(new SystemInformationLogger());
        }
    }
}
