using System;

namespace ESMLoop.LoggingData
{
    internal class QuestionLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;

        public int[] Flow;

        public int[] NasaTLX;

        public string DropDownAnswer;

        public string Description;

        public QuestionLoggingData(int[] Flow, int[] NasaTLX, string DropDownAnswer, string Description)
        {
            SystemTime = DateTime.Now;
            this.Flow = Flow;
            this.NasaTLX = NasaTLX;
            this.DropDownAnswer = DropDownAnswer;
            this.Description = Description;
        }

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                Flow.ToCSVString(),
                NasaTLX.ToCSVString(),
                DropDownAnswer,
                Description
            };
            return content.ToCSVString();
        }
    }
}
