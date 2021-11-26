using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandelTSE.DatensicherungEinstellungen
{
    /// <summary>
    /// Interaction logic for ODBCDatensicherung.xaml
    /// </summary>
    public partial class ODBCDatensicherung : UserControl
    {
        public ODBCDatensicherung()
        {
            InitializeComponent();

            var image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Exclamation.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            icon.Source = image;
        }

        private void Standard_Click(object sender, RoutedEventArgs e)
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
                    Content = new Datensicherung();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}
