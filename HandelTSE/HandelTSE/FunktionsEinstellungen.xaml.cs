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

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for Funktionseinstellungen.xaml
    /// </summary>
    public partial class FunktionsEinstellungen : UserControl
    {
        public FunktionsEinstellungen()
        {
            InitializeComponent();
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void speichernCommonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void EinmannbetriebCheckBox_Unchecked(object sender, RoutedEventArgs e) { AnmeldungMitPasswortCheckbox.IsEnabled = false; if (AnmeldungMitPasswortCheckbox.IsChecked == false) AnmeldungMitPasswortCheckbox.IsChecked = true; AnmeldungMitPasswortTextBlock.Opacity = 0.5; }

        private void EinmannbetriebCheckBox_Checked(object sender, RoutedEventArgs e) { AnmeldungMitPasswortCheckbox.IsEnabled = true; AnmeldungMitPasswortTextBlock.Opacity = 1; }

        private void AbholscheineCheckbox_Checked(object sender, RoutedEventArgs e) { AbholdatumCheckbox.IsEnabled = true; AbholdatumTextBlock.Opacity = 1; VollstandigCheckbox.IsEnabled = true; KleinerZettelCheckbox.IsEnabled = true; }

        private void AbholscheineCheckbox_Unchecked(object sender, RoutedEventArgs e) { AbholdatumCheckbox.IsEnabled = false; AbholdatumCheckbox.IsChecked = false; AbholdatumTextBlock.Opacity = 0.5; VollstandigCheckbox.IsEnabled = false; KleinerZettelCheckbox.IsEnabled = false; if (VollstandigCheckbox.IsChecked == true) KleinerZettelCheckbox.IsChecked = true; }

        private void VollstandigCheckbox_Checked(object sender, RoutedEventArgs e) { if (KleinerZettelCheckbox.IsChecked == true) KleinerZettelCheckbox.IsChecked = false; }

        private void KleinerZettelCheckbox_Checked(object sender, RoutedEventArgs e) { if (VollstandigCheckbox.IsChecked == true) VollstandigCheckbox.IsChecked = false; }

        private void Bondrucker_Checked(object sender, RoutedEventArgs e) { if (A4DruckerCheckbox.IsChecked == true) A4DruckerCheckbox.IsChecked = false; }
        private void A4Drucker_Checked(object sender, RoutedEventArgs e) { if (BondruckerCheckbox.IsChecked == true) BondruckerCheckbox.IsChecked = false; }

        private void ZAbschlagSenden_Checked(object sender, RoutedEventArgs e) { AbschlagSendenTextBox.IsEnabled = true; }
        private void ZAbschlagSenden_Unchecked(object sender, RoutedEventArgs e) { AbschlagSendenTextBox.Text = ""; AbschlagSendenTextBox.IsEnabled = false; }

        private void ZAbschlagDrucken_Checked(object sender, RoutedEventArgs e)
        {
            BondruckerCheckbox.IsChecked = true;
            A4DruckerCheckbox.IsChecked = false;
            BondruckerCheckbox.IsEnabled = true;
            A4DruckerCheckbox.IsEnabled = true;
            BondruckerTextBlock.Opacity = 1;
            A4DruckerTextBlock.Opacity = 1;
        }

        private void ZAbschlagDrucken_Unchecked(object sender, RoutedEventArgs e)
        {
            BondruckerCheckbox.IsChecked = false;
            A4DruckerCheckbox.IsChecked = false;
            BondruckerCheckbox.IsEnabled = false;
            A4DruckerCheckbox.IsEnabled = false;
            BondruckerTextBlock.Opacity = 0.5;
            A4DruckerTextBlock.Opacity = 0.5;
        }

        private void A4Drucker_Unchecked(object sender, RoutedEventArgs e) { if (BondruckerCheckbox.IsChecked == false && ZAbschlagDrucken.IsChecked == true) A4DruckerCheckbox.IsChecked = true; }
        private void Bondrucker_Unchecked(object sender, RoutedEventArgs e) { if (A4DruckerCheckbox.IsChecked == false && ZAbschlagDrucken.IsChecked == true) BondruckerCheckbox.IsChecked = true; }

        private void VollstandigCheckbox_Unchecked(object sender, RoutedEventArgs e) { if (KleinerZettelCheckbox.IsChecked == false && AbholscheineCheckbox.IsChecked == true) VollstandigCheckbox.IsChecked = true; }
        private void KleinerZettelCheckbox_Unchecked(object sender, RoutedEventArgs e) { if (VollstandigCheckbox.IsChecked == false && AbholscheineCheckbox.IsChecked == true) KleinerZettelCheckbox.IsChecked = true; }

        private void EinzweckRadioButton_Checked(object sender, RoutedEventArgs e) { EinzweckComboBox.IsEnabled = true; EinzweckComboBox.Opacity = 1; }

        private void MehrzweckRadioButton_Checked(object sender, RoutedEventArgs e) { EinzweckComboBox.IsEnabled = false; EinzweckComboBox.Opacity = 0.7; }

        private void GutscheinSystem_Unchecked(object sender, RoutedEventArgs e) { GutscheinTextBox.IsEnabled = false; }

        private void GutscheinSystem_Checked(object sender, RoutedEventArgs e) { GutscheinTextBox.IsEnabled = true; }

        private void InfoButton_Clicked(object sender, RoutedEventArgs e) { if (InfoWindow.Visibility == Visibility.Collapsed) { InfoWindow.Visibility = Visibility.Visible; return; } else if (InfoWindow.Visibility == Visibility.Visible) InfoWindow.Visibility = Visibility.Collapsed; }

        private void InfoWindow_CloseButtonClicked(object sender, RoutedEventArgs e) { InfoWindow.Visibility = Visibility.Collapsed; }
    }
}
