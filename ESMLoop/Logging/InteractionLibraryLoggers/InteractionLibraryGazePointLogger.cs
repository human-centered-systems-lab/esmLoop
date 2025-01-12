using ESMLoop.LoggingData;
using System;
using Tobii.InteractionLib;

namespace ESMLoop.Logging.InteractionLibraryLoggers
{
    internal class InteractionLibraryGazePointLogger : AbstractLeafLogger<GazePointLoggingData>
    {
        private readonly IInteractionLib _intlib;

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "DeviceTime",
            "Validity",
            "X",
            "Y"
        }.ToCSVString();

        internal InteractionLibraryGazePointLogger(IInteractionLib intlib)
        {
            _intlib = intlib;
        }

        public override void Start(DateTime currentTime)
        {
            base.Start(currentTime);

            _intlib.GazePointDataEvent += evt =>
            {
                if (evt.timestamp_us == 0) return;
                LoggingData.Enqueue(new(
                    time: evt.timestamp_us,
                    Val: evt.validity,
                    xPoint: evt.x,
                    yPoint: evt.y
                ));
            };
        }
    }
}
