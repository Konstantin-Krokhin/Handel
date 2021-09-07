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
    }
}
