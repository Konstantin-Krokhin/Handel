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
    /// Interaction logic for BonRechnung.xaml
    /// </summary>
    public partial class BonRechnung : UserControl
    {
        public BonRechnung()
        {
            InitializeComponent();
        }

        private void LeftAlignment1Button_Click(object sender, RoutedEventArgs e)
        {
            KopfzeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Left;
        }

        private void CenterAlignment1Button_Click(object sender, RoutedEventArgs e)
        {
            KopfzeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        private void LeftAlignment2Button_Click(object sender, RoutedEventArgs e)
        {
            FusszeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Left;
        }

        private void CenterAlignment2Button_Click(object sender, RoutedEventArgs e)
        {
            FusszeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
        }
    }
}
