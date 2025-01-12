using ESMLoop.LoggingData;
using System;
using Tobii.InteractionLib;

namespace ESMLoop.Logging.InteractionLibraryLoggers
{
    internal class InteractionLibraryPresenceLogger : AbstractLeafLogger<PresenceLoggingData>
    {
        private readonly IInteractionLib _intlib;

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "DeviceTime",
            "Presence"
        }.ToCSVString();

        internal InteractionLibraryPresenceLogger(IInteractionLib intlib)
        {
            _intlib = intlib;
        }

        public override void Start(DateTime currentTime)
        {
            base.Start(currentTime);

            _intlib.PresenceDataEvent += evt =>
            {
                if (evt.timestamp_us == 0) return;
                LoggingData.Enqueue(new(
                    timestamp_us: evt.timestamp_us,
                    presence: evt.presence
                ));
            };
        }
    }
}
