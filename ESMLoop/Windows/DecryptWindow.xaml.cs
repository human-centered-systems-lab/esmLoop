using ESMLoop.Encryption;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace ESMLoop.Windows
{
    /// <summary>
    /// Interaction logic for DecryptWindow.xaml
    /// </summary>
    public partial class DecryptWindow : Window
    {
        private string _folderPath = String.Empty;
        public DecryptWindow()
        {
            InitializeComponent();
        }
        private void ButtonSetLocation_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            _folderPath = dialog.SelectedPath;
        }
        private void ButtonDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(_folderPath))
            {
                System.Windows.MessageBox.Show("Bitte Pfad festlegen");
                return;
            }
            Encryptor.DecryptLoggingData(_folderPath);
            System.Windows.MessageBox.Show("Erfolgreich decrypted!");
        }
        void DecryptWindow_Closed(object sender, EventArgs e)
        {
            ((App)System.Windows.Application.Current).Shutdown();
        }
    }
}
