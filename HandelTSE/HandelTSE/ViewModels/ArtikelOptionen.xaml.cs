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

namespace HandelTSE.ViewModels//.Artikelverwaltung
{
    /// <summary>
    /// Interaction logic for ArtikelOptionen.xaml
    /// </summary>
    
    public partial class ArtikelOptionen
    {
        public ArtikelOptionen()
        {
            InitializeComponent();
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e) { (this.Parent as Canvas).Children.Remove(this); }
    }
}
