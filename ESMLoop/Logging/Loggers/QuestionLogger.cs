using ESMLoop.Logging.SoftwareLoggers;
using ESMLoop.LoggingData;
using ESMLoop.Properties;
using ESMLoop.Windows;
using System;
using System.Timers;
using System.Windows;

namespace ESMLoop.Logging.Loggers
{
    internal class QuestionLogger : AbstractLeafLogger<QuestionLoggingData>
    {
        ////Debugging
        private const int MIN_TIME_MILLISECONDS = 5000; //(int)1.8e+6;
        private const int MAX_TIME_MILLISECONDS = 10000; //(int)3.6e+6;

        ////Experiment
        //private const int MIN_TIME_MILLISECONDS = 20 * 60000; //20 min;
        //private const int MAX_TIME_MILLISECONDS = 60 * 60000; //60 min

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "Flow0", "Flow1", "Flow2", "Flow3", "Flow4", "Flow5", "Flow6", "Flow7", "Flow8", "Flow9", "Flow10", "Flow11", "Flow12",
            "Nasa0", "Nasa1", "Nasa2", "Nasa3", "Nasa4", "Nasa5",
            "DropDownAnswer",
            "Description"
        }.ToCSVString();

        private readonly Timer _timer;
        private readonly Random _random;

        internal QuestionLogger()
        {
            _timer = new Timer()
            {
                AutoReset = false,
                Enabled = false
            };
            _timer.Elapsed += Question;
            _random = new Random();
        }

        public override void Start(DateTime currentTime)
        {
            base.Start(currentTime);
            StartTimer();
        }
        public override void Stop()
        {
            _timer.Stop();
            _timer.Dispose();
            base.Stop();
        }

        private void Question(Object? source, ElapsedEventArgs? e)
        {
            DateTime dateNow = DateTime.Now;
            Debug.Log("QuestionnaireIssued" + dateNow.ToString());
            Settings.Default.QuestionnaireTime = dateNow;
            Settings.Default.Save();
            _timer.Stop();
            var result = MessageBox.Show("Haben Sie jetzt Zeit einen kurzen Fragebogen zu beantworten?", "esmLoop", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            if (result != MessageBoxResult.Yes)
            {
                StartTimer();
                return;
            }
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                new Questioning(this).Show();
            });
        }

        private void StartTimer()
        {
            _timer.Interval = _random.Next(MIN_TIME_MILLISECONDS, MAX_TIME_MILLISECONDS);
            _timer.Start();
        }

        #region WindowCallbacks

        internal void SendAnswers(int[]? flow, int[]? nasa, Question3Answer q3Answer)
        {
            if (flow == null || nasa == null) throw new NullReferenceException();
            LoggingData.Enqueue(new(
                Flow: flow,
                NasaTLX: nasa,
                DropDownAnswer: q3Answer.DropDownAnswer,
                Description: q3Answer.TextBoxDescription));
            Log();

            Settings.Default.LabelCounterToday++;
            Settings.Default.LabelCounter++;
            Settings.Default.Save();

            StartTimer();
        }

        public void Cancel()
        {
            StartTimer();
        }
        #endregion
    }
}
