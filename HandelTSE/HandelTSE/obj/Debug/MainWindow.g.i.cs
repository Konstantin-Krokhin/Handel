﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "954A1338FFB2AB4E870E84163A3DDE1397A33E816EF1F4E092801484BF71C9B2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HandelTSE;
using HandelTSE.ViewModels;
using HandelTSE.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
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


namespace HandelTSE {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 58 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Kasse;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Artikelverwaltung;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ArtikelOptionen;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem EanEinstellungen;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock dateBlock;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dg1;
        
        #line default
        #line hidden
        
        
        #line 164 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dg2;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl ContentWindow;
        
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
            System.Uri resourceLocater = new System.Uri("/HandelTSE;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
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
            
            #line 12 "..\..\MainWindow.xaml"
            ((HandelTSE.MainWindow)(target)).Closed += new System.EventHandler(this.Close_Clicked);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Kasse = ((System.Windows.Controls.MenuItem)(target));
            
            #line 58 "..\..\MainWindow.xaml"
            this.Kasse.Click += new System.Windows.RoutedEventHandler(this.Kasse_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Artikelverwaltung = ((System.Windows.Controls.MenuItem)(target));
            
            #line 61 "..\..\MainWindow.xaml"
            this.Artikelverwaltung.Click += new System.Windows.RoutedEventHandler(this.Artikelverwaltung_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 75 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ProgramBeenden_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 110 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ProgramEinstellungen_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 111 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.FunktionsEnstellungen_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 112 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Personalverwaltung_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ArtikelOptionen = ((System.Windows.Controls.MenuItem)(target));
            
            #line 113 "..\..\MainWindow.xaml"
            this.ArtikelOptionen.Click += new System.Windows.RoutedEventHandler(this.ArtikelOptionen_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 114 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Umsatzsteuer_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 115 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Zahlungen_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.EanEinstellungen = ((System.Windows.Controls.MenuItem)(target));
            
            #line 116 "..\..\MainWindow.xaml"
            this.EanEinstellungen.Click += new System.Windows.RoutedEventHandler(this.EanEinstellungen_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 117 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Stornogrunde_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 118 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.PresseUndVMP_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.dateBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 15:
            this.dg1 = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 16:
            this.dg2 = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 17:
            
            #line 167 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.MainWindow_Clicked);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 168 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.StatTable_Clicked);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 169 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.StatGraph_Clicked);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 170 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.TransParking_Clicked);
            
            #line default
            #line hidden
            return;
            case 21:
            
            #line 171 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.MwStTool_Clicked);
            
            #line default
            #line hidden
            return;
            case 22:
            this.ContentWindow = ((System.Windows.Controls.ContentControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

