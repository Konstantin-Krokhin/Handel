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

        private void KundenanzeigeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            AnschlussComboBox.IsEnabled = true;
            AnschlusstestenButton.IsEnabled = true;
            BegrussungstextButton.IsEnabled = true;
            EinstellungenButton.IsEnabled = true;
        }

        private void KundenanzeigeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            AnschlussComboBox.IsEnabled = false;
            AnschlusstestenButton.IsEnabled = false;
            BegrussungstextButton.IsEnabled = false;
            EinstellungenButton.IsEnabled = false;
        }

        private void KundendisplayCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            KundendisplayButton.IsEnabled = false;
            BTextBox.IsEnabled = false;
            HTextBox.IsEnabled = false;
            HintergrundbildTextBox.IsEnabled = false;
            runterladenButton.IsEnabled = false;
            BildBTextBox.IsEnabled = false;
            BildHTextBox.IsEnabled = false;
            LogoTextBox.IsEnabled = false;
            logoRunterladenButton.IsEnabled = false;
            BegrüßungTextBox.IsEnabled = false;
            BegrussungNachBondruckTextBox.IsEnabled = false;
        }

        private void KundendisplayCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            KundendisplayButton.IsEnabled = true;
            BTextBox.IsEnabled = true;
            HTextBox.IsEnabled = true;
            HintergrundbildTextBox.IsEnabled = true;
            runterladenButton.IsEnabled = true;
            BildBTextBox.IsEnabled = true;
            BildHTextBox.IsEnabled = true;
            LogoTextBox.IsEnabled = true;
            logoRunterladenButton.IsEnabled = true;
            BegrüßungTextBox.IsEnabled = true;
            BegrussungNachBondruckTextBox.IsEnabled = true;
        }
    }
}
