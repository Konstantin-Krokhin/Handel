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

namespace HandelTSE.ViewModels
{
    /// <summary>
    /// Interaction logic for GerateManager.xaml
    /// </summary>
    public partial class GerateManager : UserControl
    {
        public GerateManager()
        {
            InitializeComponent();
        }

        private void GerateManagerTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void outRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PasswordRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void A4DruckerCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (A4DruckerCheckbox.IsChecked == true)
            {
                A4PrinterComboBox.IsEnabled = true;
                A4testenButton.IsEnabled = true;
            }
        }

        private void EtikettenDruckerCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            EtikettPrinterComboBox.IsEnabled = true;
            EtikettButton.IsEnabled = true;
            VorlageTextBox.IsEnabled = true;
            VorlageButton.IsEnabled = true;
            VorschauButton.IsEnabled = true;
        }

        private void A4DruckerCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            A4PrinterComboBox.IsEnabled = false;
            A4testenButton.IsEnabled = false;
        }

        private void EtikettenDruckerCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            EtikettPrinterComboBox.IsEnabled = false;
            EtikettButton.IsEnabled = false;
            VorlageTextBox.IsEnabled = false;
            VorlageButton.IsEnabled = false;
            VorschauButton.IsEnabled = false;
        }
    }
}
