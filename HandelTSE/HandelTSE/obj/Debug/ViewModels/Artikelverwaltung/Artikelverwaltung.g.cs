﻿#pragma checksum "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E960FAA90C247FEFF4330AEEE6FB06CAD9FDB738C7AE642BC83ED40579B12302"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HandelTSE.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Converters;
using Xceed.Wpf.Toolkit.Core;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Mag.Converters;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace HandelTSE.ViewModels {
    
    
    /// <summary>
    /// Artikelverwaltung
    /// </summary>
    public partial class Artikelverwaltung : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 14 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView TreeView;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox gruppe;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.ColorPicker cp;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchBoxArtikel;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Artikel;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Lieferanten;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PluEan;
        
        #line default
        #line hidden
        
        
        #line 157 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ArtikelOptionenButton;
        
        #line default
        #line hidden
        
        
        #line 158 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ArtikelCodeTemplate;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SetArtikelButton;
        
        #line default
        #line hidden
        
        
        #line 160 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ArtikelCodeTemplateValue;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox VKPreisBrutto;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EKPreisBrutto;
        
        #line default
        #line hidden
        
        
        #line 170 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox VKPreisNetto;
        
        #line default
        #line hidden
        
        
        #line 173 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EKPreisNetto;
        
        #line default
        #line hidden
        
        
        #line 180 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AusserHaus;
        
        #line default
        #line hidden
        
        
        #line 184 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ImHaus;
        
        #line default
        #line hidden
        
        
        #line 188 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Einheit;
        
        #line default
        #line hidden
        
        
        #line 202 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SortNr;
        
        #line default
        #line hidden
        
        
        #line 205 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Legerplatz1;
        
        #line default
        #line hidden
        
        
        #line 206 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Legerplatz2;
        
        #line default
        #line hidden
        
        
        #line 209 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Pfand;
        
        #line default
        #line hidden
        
        
        #line 223 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Bestand;
        
        #line default
        #line hidden
        
        
        #line 226 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Etikett1;
        
        #line default
        #line hidden
        
        
        #line 228 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox Seriennummer;
        
        #line default
        #line hidden
        
        
        #line 232 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Bestellmenge;
        
        #line default
        #line hidden
        
        
        #line 235 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Etikett2;
        
        #line default
        #line hidden
        
        
        #line 237 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox MengeEingabe;
        
        #line default
        #line hidden
        
        
        #line 241 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MindestBestand;
        
        #line default
        #line hidden
        
        
        #line 243 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EtikettDruckenButton;
        
        #line default
        #line hidden
        
        
        #line 250 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button KopierenInWGButton;
        
        #line default
        #line hidden
        
        
        #line 254 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button VerschiebenInWGButton;
        
        #line default
        #line hidden
        
        
        #line 258 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox WGComboBox;
        
        #line default
        #line hidden
        
        
        #line 260 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button KopieSpeichernButton;
        
        #line default
        #line hidden
        
        
        #line 274 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox AlleArtikel;
        
        #line default
        #line hidden
        
        
        #line 277 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox AuserHausCheckbox;
        
        #line default
        #line hidden
        
        
        #line 279 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AuserHausComboBox2;
        
        #line default
        #line hidden
        
        
        #line 281 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ImHausCheckbox;
        
        #line default
        #line hidden
        
        
        #line 283 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ImHausComboBox2;
        
        #line default
        #line hidden
        
        
        #line 286 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dg3;
        
        #line default
        #line hidden
        
        
        #line 291 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn temp;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/HandelTSE;component/viewmodels/artikelverwaltung/artikelverwaltung.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.TreeView = ((System.Windows.Controls.TreeView)(target));
            
            #line 52 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.TreeView.AddHandler(System.Windows.Controls.TreeViewItem.SelectedEvent, new System.Windows.RoutedEventHandler(this.TreeViewItem_OnItemSelected));
            
            #line default
            #line hidden
            
            #line 52 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.TreeView.Loaded += new System.Windows.RoutedEventHandler(this.LoadForm);
            
            #line default
            #line hidden
            return;
            case 3:
            this.gruppe = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            
            #line 70 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CleanGroupButton);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 77 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ColorOfWGChange);
            
            #line default
            #line hidden
            return;
            case 6:
            this.cp = ((Xceed.Wpf.Toolkit.ColorPicker)(target));
            
            #line 83 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.cp.SelectedColorChanged += new System.Windows.RoutedPropertyChangedEventHandler<System.Nullable<System.Windows.Media.Color>>(this.cp_SelectedColorChanged_1);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 85 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WarenGruppeSave);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 91 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EAN_SearchButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 99 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.TextSearchButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 107 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CleanFormButton);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 115 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ArtikelDelete_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 123 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AlleArtikelnDelete_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 131 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WarenGruppeDelete_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.SearchBoxArtikel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.Artikel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 16:
            this.Lieferanten = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 17:
            this.PluEan = ((System.Windows.Controls.ComboBox)(target));
            
            #line 149 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.PluEan.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.PluEan_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 18:
            this.ArtikelOptionenButton = ((System.Windows.Controls.Button)(target));
            return;
            case 19:
            this.ArtikelCodeTemplate = ((System.Windows.Controls.ComboBox)(target));
            
            #line 158 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.ArtikelCodeTemplate.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ArtikelCodeTemplate_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 20:
            this.SetArtikelButton = ((System.Windows.Controls.Button)(target));
            return;
            case 21:
            this.ArtikelCodeTemplateValue = ((System.Windows.Controls.TextBox)(target));
            return;
            case 22:
            this.VKPreisBrutto = ((System.Windows.Controls.TextBox)(target));
            return;
            case 23:
            this.EKPreisBrutto = ((System.Windows.Controls.TextBox)(target));
            return;
            case 24:
            this.VKPreisNetto = ((System.Windows.Controls.TextBox)(target));
            return;
            case 25:
            this.EKPreisNetto = ((System.Windows.Controls.TextBox)(target));
            return;
            case 26:
            this.AusserHaus = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 27:
            this.ImHaus = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 28:
            this.Einheit = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 29:
            this.SortNr = ((System.Windows.Controls.TextBox)(target));
            return;
            case 30:
            this.Legerplatz1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 31:
            this.Legerplatz2 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 32:
            this.Pfand = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 33:
            this.Bestand = ((System.Windows.Controls.TextBox)(target));
            return;
            case 34:
            this.Etikett1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 35:
            this.Seriennummer = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 36:
            this.Bestellmenge = ((System.Windows.Controls.TextBox)(target));
            return;
            case 37:
            this.Etikett2 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 38:
            this.MengeEingabe = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 39:
            this.MindestBestand = ((System.Windows.Controls.TextBox)(target));
            return;
            case 40:
            this.EtikettDruckenButton = ((System.Windows.Controls.Button)(target));
            
            #line 243 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.EtikettDruckenButton.Click += new System.Windows.RoutedEventHandler(this.SaveButton);
            
            #line default
            #line hidden
            return;
            case 41:
            this.KopierenInWGButton = ((System.Windows.Controls.Button)(target));
            
            #line 250 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.KopierenInWGButton.Click += new System.Windows.RoutedEventHandler(this.KopierenInWGButton_Clicked);
            
            #line default
            #line hidden
            return;
            case 42:
            this.VerschiebenInWGButton = ((System.Windows.Controls.Button)(target));
            
            #line 254 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.VerschiebenInWGButton.Click += new System.Windows.RoutedEventHandler(this.VerschiebenInWGButton_Clicked);
            
            #line default
            #line hidden
            return;
            case 43:
            this.WGComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 258 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.WGComboBox.IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.WGComboLoadElements);
            
            #line default
            #line hidden
            return;
            case 44:
            this.KopieSpeichernButton = ((System.Windows.Controls.Button)(target));
            
            #line 260 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.KopieSpeichernButton.Click += new System.Windows.RoutedEventHandler(this.SaveButton);
            
            #line default
            #line hidden
            return;
            case 45:
            
            #line 267 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveButton);
            
            #line default
            #line hidden
            return;
            case 46:
            this.AlleArtikel = ((System.Windows.Controls.CheckBox)(target));
            
            #line 274 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.AlleArtikel.Checked += new System.Windows.RoutedEventHandler(this.CheckedBox_SelectAll);
            
            #line default
            #line hidden
            
            #line 274 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.AlleArtikel.Unchecked += new System.Windows.RoutedEventHandler(this.CheckedBox_DeselectAll);
            
            #line default
            #line hidden
            return;
            case 47:
            this.AuserHausCheckbox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 48:
            this.AuserHausComboBox2 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 49:
            this.ImHausCheckbox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 50:
            this.ImHausComboBox2 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 51:
            this.dg3 = ((System.Windows.Controls.DataGrid)(target));
            
            #line 286 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.dg3.AutoGeneratingColumn += new System.EventHandler<System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs>(this.CustomizeHeaders);
            
            #line default
            #line hidden
            
            #line 289 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            this.dg3.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dg3_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 52:
            this.temp = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 53:
            
            #line 294 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.UncheckedBox);
            
            #line default
            #line hidden
            
            #line 294 "..\..\..\..\ViewModels\Artikelverwaltung\Artikelverwaltung.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckedBox);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

