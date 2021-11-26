using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using HandelTSE.DatensicherungEinstellungen;

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for Datensicherung.xaml
    /// </summary>
    public partial class Datensicherung : UserControl
    {
        public Datensicherung()
        {
            InitializeComponent();

            var image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Information.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            icon.Source = image;
        }

        private void RecordSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DatenbankDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OpenDirectory_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Offen";
            openFileDialog.Filter = "Alle Dateien *.*|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                DatenbankDirectoryTextBox.Text = openFileDialog.FileName;
            }
        }

        private void SaveDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                SaveDirectoryTextBox.Text = dialog.SelectedPath;
            }
        }

        private void ODBC_Clicked(object sender, RoutedEventArgs e) 
        {
            string caption = "Information";
            string messageBoxText = "Wollen Sie wirklich die Daten-Einstellungen ändern?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.None;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    Content = new ODBCDatensicherung();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}
