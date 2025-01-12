namespace ESMLoop.LoggingData
{
    internal class SystemInformationLoggingData : AbstractLoggingData
    {
        public int ScreenWidth;
        public int ScreenHeight;
        public string ScreenName;

        public SystemInformationLoggingData(int screenWidth, int screenHeight, string screenName)
        {
            ScreenName = screenName;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
        }

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                ScreenWidth.ToString(),
                ScreenHeight.ToString(),
                ScreenName
            };
            return content.ToCSVString();
        }
    }
}
