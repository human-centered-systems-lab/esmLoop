using ESMLoop.Logging;
using ESMLoop.Properties;
using ESMLoop.Windows;
using Microsoft.Win32;
using Sentry;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using Debug = ESMLoop.Logging.SoftwareLoggers.Debug;
using Forms = System.Windows.Forms;

namespace ESMLoop
{
    /// <summary>   
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal readonly LoggingManager LoggingManager;
        private readonly Forms.NotifyIcon _notify;

        internal bool WindowOpen;

        public App()
        {
            //DispatcherUnhandledException += App_DispatcherUnhandledException;
            //SentrySdk.Init(o =>
            //{
            //    // Tells which project in Sentry to send events to:
            //    o.Dsn = "https://eec4e01f936b4ef7849876e0b799457e@o1430464.ingest.sentry.io/6781779";
            //    // When configuring for the first time, to see what the SDK is doing:
            //    o.Debug = false;
            //    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
            //    // We recommend adjusting this value in production.
            //    o.TracesSampleRate = 0.1;
            //    o.IsGlobalModeEnabled = true;
            //});

            LoggingManager = new LoggingManager();
            _notify = new Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon("Resources/Icon.ico"),
                Visible = true,
                Text = "esmLoop",
                ContextMenuStrip = new Forms.ContextMenuStrip(),
            };

            _notify.ContextMenuStrip.Items.Add("Open Dashboard", null, ContextMenu_OnClickDashboard);
            _notify.ContextMenuStrip.Items.Add("Set up Display", null, ContextMenu_OnClickScreenSetup);
            _notify.ContextMenuStrip.Items.Add("Calibrate Eye Tracker", null, ContextMenu_OnClickCalibration);
            _notify.ContextMenuStrip.Items.Add("Start Experience Sampling Session", null, ContextMenu_OnClickStartRecording);
            _notify.ContextMenuStrip.Items.Add("Stop Experience Sampling Session", null, ContextMenu_OnClickStopRecording);


            //Settings.Default.LabelCounterToday = 0;
            DateTime dateNow = DateTime.Now;
            if (Settings.Default.StartTime.ToString("ddMMyyyy") != dateNow.ToString("ddMMyyyy")) Settings.Default.LabelCounterToday = 0;
            Settings.Default.UserID = "2003";
            Settings.Default.Save();

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);


        }

        private void ContextMenu_OnClickDashboard(object? sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("https://m0m0123.github.io/Dashboard_2003_D/") { UseShellExecute = true });
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            SentrySdk.CaptureException(e.Exception);

            // If you want to avoid the application from crashing:
            e.Handled = true;
        }

        void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            //if (e.Reason == SessionSwitchReason.SessionLock)
            //{
            //    //I left my desk
            //}
            if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                MessageBox.Show("Bitte überprüfen Sie, ob der Eye Tracker noch rot leuchtet und ob in der TobiiProSDK-Datei noch Daten aufgezeichnet werden. Falls nicht, starten Sie die Anwendung neu.", "esmLoop", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PreventSleep();
            new DecryptWindow().Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LoggingManager.Log();
            LoggingManager.Encrypt();
            Settings.Default.Save();
            _notify.Dispose();
            AllowSleep();
            base.OnExit(e);
        }

        #region Sleep

        public static void PreventSleep()
        {
            SetThreadExecutionState(ExecutionState.EsDisplayRequired | ExecutionState.EsContinuous);
        }

        public static void AllowSleep()
        {
            SetThreadExecutionState(ExecutionState.EsContinuous);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        [Flags]
        private enum ExecutionState : uint
        {
            EsAwaymodeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            EsSystemRequired = 0x00000001
        }

        #endregion
        #region ContextMenu Lister

        private void ContextMenu_OnClickCalibration(object? sender, EventArgs e)
        {
            Debug.Log("Calibrate");
            Eyetracker.Calibrate();
        }

        private void ContextMenu_OnClickScreenSetup(object? sender, EventArgs e)
        {
            Debug.Log("Display setup");
            Eyetracker.ScreenSetup();
        }

        private void ContextMenu_OnClickStopRecording(object? sender, EventArgs e)
        {
            Debug.Log("Stop Recording");
            var result = MessageBox.Show("Wollen sie die Experience Sampling Session wirklich beenden?", "esmLoop", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.No) return;
            if (result == MessageBoxResult.Yes)
            {
                LoggingManager.Stop();
                Application.Current.Shutdown();
                //TODO entfernen sehr unschön
                foreach (var process in Process.GetProcesses().Where(p => p.ProcessName.Contains("ESMLoop")))
                {
                    process.Kill();
                }

            }

        }

        private void ContextMenu_OnClickStartRecording(object? sender, EventArgs e)
        {
            Debug.Log("Start Recording");
            if (LoggingManager.IsRunning)
            {
                MessageBox.Show("Die Anwendung läuft bereits!", "esmLoop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (WindowOpen)
            {
                MainWindow.Activate();
            }
            else
            {
                new MainWindow().Show();
            }
        }

        #endregion
    }
}
