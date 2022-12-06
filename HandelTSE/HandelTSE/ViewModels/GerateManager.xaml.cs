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



        private void BondruckerCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            BondruckerComboBox.IsEnabled = true;
            BondruckertestenButton.IsEnabled = true;
            weitereEinstellungenButton.IsEnabled = true;
            GeldKassenladeGroupBox.IsEnabled = true;
            GeldKassenladeCheckBox.IsEnabled = true;
        }

        private void BondruckerCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            BondruckerComboBox.IsEnabled = false;
            BondruckertestenButton.IsEnabled = false;
            weitereEinstellungenButton.IsEnabled = false;
            GeldKassenladeGroupBox.IsEnabled = false;
            GeldKassenladeCheckBox.IsChecked = false;
            GeldKassenladeCheckBox.IsEnabled = false;
        }

        private void GeldKassenladeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SteuerungRadioButton.IsEnabled = true;
            AnderenAnschlussRadioButton.IsEnabled = true;
            anderenAnschlusstestenButton.IsEnabled = true;
            anderenAnschlussComboBox.IsEnabled = false;
            SteuerungRadioButton.IsChecked = true;
        }

        private void GeldKassenladeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SteuerungRadioButton.IsEnabled = false;
            AnderenAnschlussRadioButton.IsEnabled = false;
            anderenAnschlusstestenButton.IsEnabled = false;
            anderenAnschlussComboBox.IsEnabled = false;
        }

        private void AnschlussRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            anderenAnschlussComboBox.IsEnabled = true;
        }

        private void AnschlussRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (anderenAnschlussComboBox.IsEnabled == true) anderenAnschlussComboBox.IsEnabled = false;
        }



        private void Bondrucker2Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            Bondrucker2ComboBox.IsEnabled = true;
            Bondruckertesten2Button.IsEnabled = true;
            weitereEinstellungen2Button.IsEnabled = true;
            GeldKassenlade2GroupBox.IsEnabled = true;
            GeldKassenlade2CheckBox.IsEnabled = true;
        }

        private void Bondrucker2Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Bondrucker2ComboBox.IsEnabled = false;
            Bondruckertesten2Button.IsEnabled = false;
            weitereEinstellungen2Button.IsEnabled = false;
            GeldKassenlade2GroupBox.IsEnabled = false;
            GeldKassenlade2CheckBox.IsEnabled = false;
            GeldKassenlade2CheckBox.IsChecked = false;
        }

        private void GeldKassenlade2CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Steuerung2RadioButton.IsEnabled = true;
            AnderenAnschluss2RadioButton.IsEnabled = true;
            anderenAnschlusstesten2Button.IsEnabled = true;
            anderenAnschluss2ComboBox.IsEnabled = false;
            Steuerung2RadioButton.IsChecked = true;
        }

        private void GeldKassenlade2CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Steuerung2RadioButton.IsEnabled = false;
            AnderenAnschluss2RadioButton.IsEnabled = false;
            anderenAnschlusstesten2Button.IsEnabled = false;
            anderenAnschluss2ComboBox.IsEnabled = false;
        }

        private void Anschluss2RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            anderenAnschluss2ComboBox.IsEnabled = true;
        }

        private void Anschluss2RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (anderenAnschluss2ComboBox.IsEnabled == true) anderenAnschluss2ComboBox.IsEnabled = false;
        }



        private void Bondrucker3Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            Bondrucker3ComboBox.IsEnabled = true;
            Bondruckertesten3Button.IsEnabled = true;
            weitereEinstellungen3Button.IsEnabled = true;
            GeldKassenlade3GroupBox.IsEnabled = true;
            GeldKassenlade3CheckBox.IsEnabled = true;
        }

        private void Bondrucker3Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Bondrucker3ComboBox.IsEnabled = false;
            Bondruckertesten3Button.IsEnabled = false;
            weitereEinstellungen3Button.IsEnabled = false;
            GeldKassenlade3GroupBox.IsEnabled = false;
            GeldKassenlade3CheckBox.IsEnabled = false;
            GeldKassenlade3CheckBox.IsChecked = false;
        }

        private void GeldKassenlade3CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Steuerung3RadioButton.IsEnabled = true;
            AnderenAnschluss3RadioButton.IsEnabled = true;
            anderenAnschlusstesten3Button.IsEnabled = true;
            anderenAnschluss3ComboBox.IsEnabled = false;
            Steuerung3RadioButton.IsChecked = true;
        }

        private void GeldKassenlade3CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Steuerung3RadioButton.IsEnabled = false;
            AnderenAnschluss3RadioButton.IsEnabled = false;
            anderenAnschlusstesten3Button.IsEnabled = false;
            anderenAnschluss3ComboBox.IsEnabled = false;
        }

        private void Anschluss3RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            anderenAnschluss3ComboBox.IsEnabled = true;
        }

        private void Anschluss3RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (anderenAnschluss3ComboBox.IsEnabled == true) anderenAnschluss3ComboBox.IsEnabled = false;
        }

    }
}
