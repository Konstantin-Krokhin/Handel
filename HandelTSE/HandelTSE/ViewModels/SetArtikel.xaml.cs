﻿using System;
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
    /// Interaction logic for SetArtikel.xaml
    /// </summary>
    public partial class SetArtikel : UserControl
    {
        public SetArtikel()
        {
            InitializeComponent();
        }

        private void Text_Suchen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EAN_Suchen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WGComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listArtikelnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void SetDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackToArticle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e) { Content = new Artikelverwaltung(); }
    }
}
