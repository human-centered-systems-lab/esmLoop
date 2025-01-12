using ESMLoop.Logging;
using ESMLoop.Logging.SoftwareLoggers;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace ESMLoop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LoggingManager _loggingManager;

        public MainWindow()
        {
            InitializeComponent();
            _loggingManager = ((App)Application.Current).LoggingManager;
            ((App)Application.Current).WindowOpen = true;
        }

        private void ButtonStartRecording_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Wollen sie die Experience Sampling Session wirklich starten?", "esmLoop", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes, System.Windows.MessageBoxOptions.DefaultDesktopOnly);
            if (result == MessageBoxResult.No) return;
            if (result == MessageBoxResult.Yes)
            {
                Debug.Log("Start Recording");
                if (!_loggingManager.IsReady)
                {
                    MessageBox.Show("Bitte spezifizieren Sie einen Speicherort", "esmLoop", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //TODO: Session Time check
                _loggingManager.Start();
                this.Close();
            }
        }

        private void ButtonSetSaveLocation_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("Change save location");
            using var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            string path = dialog.SelectedPath;
            Properties.Settings.Default.Path = path;
            LabelSaveLocation.Text = path;
        }

        private void ButtonCalibrateEyetracker_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("Calibrate");
            Eyetracker.Calibrate();
        }

        private void ButtonSetupDisplay_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("Setup Display");
            Eyetracker.ScreenSetup();
        }

        private void TextBoxSessionTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            ((App)Application.Current).WindowOpen = false;
        }
    }
}
