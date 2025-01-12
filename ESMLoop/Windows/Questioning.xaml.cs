using ESMLoop.Logging.Loggers;
using ESMLoop.Logging.SoftwareLoggers;
using ESMLoop.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ESMLoop.Windows
{
    /// <summary>
    /// Interaction logic for Questioning.xaml
    /// </summary>
    public partial class Questioning : Window
    {
        private readonly Dictionary<int, UserControl> _userControls;
        private readonly Dictionary<int, TextBlock> _steps;
        private readonly int[] _order;

        private readonly QuestionLogger _questionLogger;

        private readonly Brush _highlight;
        private readonly Brush _background;

        private int _activeQuestionaire;
        private int _currentStep = 0;

        internal Questioning(QuestionLogger qLogger)
        {
            InitializeComponent();

            InitializeInformation();

            #region Define readonlys
            _questionLogger = qLogger;

            _userControls = new Dictionary<int, UserControl>()
            {
                { 1, new Question1Control() },
                { 2, new Question2Control() },
                { 3, new Question3Control() }
            };

            _steps = new Dictionary<int, TextBlock>()
            {
                { 1, Text_Step1 },
                { 2, Text_Step2 },
                { 3, Text_Step3 }
            };
            #endregion

            #region Random Question
            Random random = new();
            int first = random.Next(1, 3);
            _order = new int[3] { first, (first == 1) ? 2 : 1, 3 };
            _activeQuestionaire = first;
            this.contentControl.Content = _userControls[_activeQuestionaire];
            #endregion

            #region Step Color
            var converter = new BrushConverter();
            var highlight = converter.ConvertFromString("#FF2F75D4");
            var background = converter.ConvertFromString("#FF434546");
            if (background == null || highlight == null) throw new NullReferenceException();
            _highlight = (Brush)highlight;
            _background = (Brush)background;
            NextStep();
            #endregion
        }

        #region Question Controller
        private void NextStep()
        {
            if (_currentStep == 3) return;
            int last = _currentStep;
            _currentStep++;
            if (0 < last)
            {
                _steps[last].Background = _background;
            }
            _steps[_currentStep].Background = _highlight;
        }

        private void NextQuestion(int active)
        {
            if (_currentStep == 3) return;
            NextStep();
            _activeQuestionaire = _order[_currentStep - 1];
            this.contentControl.Content = _userControls[_activeQuestionaire];

            if (_activeQuestionaire == 3)
            {
                ButtonNext.Content = "Fertig";
                Application.Current.MainWindow.Height = 660;
            }

        }

        private static void NotAnswerd()
        {
            Debug.Log("Error not answered");
            MessageBox.Show("Bitte beantworten Sie alle Fragen", "esmLoop", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region Buttons

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _questionLogger.Cancel();
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _questionLogger.Cancel();
            this.Close();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            switch (_activeQuestionaire)
            {
                case 1:
                    {
                        var question1 = (Question1Control)_userControls[1];
                        if (!question1.Answered())
                        {
                            NotAnswerd();
                            return;
                        }
                        NextQuestion(_activeQuestionaire);
                    }
                    break;
                case 2:
                    {
                        var question2 = (Question2Control)_userControls[2];
                        if (!question2.Answered())
                        {
                            NotAnswerd();
                            return;
                        }
                        NextQuestion(_activeQuestionaire);
                    }
                    break;
                case 3:
                    {
                        var question3 = (Question3Control)_userControls[3];
                        if (!question3.Answered())
                        {
                            NotAnswerd();
                            return;
                        }
                        var question1 = (Question1Control)_userControls[1];
                        var question2 = (Question2Control)_userControls[2];
                        _questionLogger.SendAnswers(question1.Answer(), question2.Answer(), question3.Answer());
                        this.Close();
                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Information
        private void InitializeInformation()
        {
            const long factor = (long)1e+6;

            LabelsTotal.Text = Settings.Default.LabelCounter.ToString();
            LabelsToday.Text = Settings.Default.LabelCounterToday.ToString();

            string path = Settings.Default.Path;

            DataTotal.Text = (GetDirectorySize(path, "*") / factor).ToString() + " MB";

            string fileNames = "*" + Settings.Default.StartTime.ToString("yyMMdd") + "*";
            DataToday.Text = (GetDirectorySize(path, fileNames) / factor).ToString() + " MB";
        }
        private static long GetDirectorySize(string folderPath, string fileName)
        {
            DirectoryInfo di = new(folderPath);
            return di.EnumerateFiles(fileName, SearchOption.AllDirectories).Sum(fi => fi.Length);
        }
        #endregion
    }
}
