﻿#pragma checksum "..\..\Zahlungen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1A74CBF0051B18F35C3DE8478E508264F0757EE9E38088808D1C3D66272555A4"
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


namespace HandelTSE {
    
    
    /// <summary>
    /// Zahlungen
    /// </summary>
    public partial class Zahlungen : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\Zahlungen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NeuZahlungsmethodeButton;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\Zahlungen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ZahlungenDataGrid;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\Zahlungen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.Primitives.WindowControl NeuZahlungWindow;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\Zahlungen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Zahlungsname;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\Zahlungen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Zahlungsart;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\Zahlungen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Bemerkung;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\Zahlungen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button StatusButton;
        
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
            System.Uri resourceLocater = new System.Uri("/HandelTSE;component/zahlungen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Zahlungen.xaml"
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
            this.NeuZahlungsmethodeButton = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\Zahlungen.xaml"
            this.NeuZahlungsmethodeButton.Click += new System.Windows.RoutedEventHandler(this.NeuZahlungsmethode_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ZahlungenDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 35 "..\..\Zahlungen.xaml"
            this.ZahlungenDataGrid.Loaded += new System.Windows.RoutedEventHandler(this.ZahlungenDataGrid_Loaded);
            
            #line default
            #line hidden
            
            #line 35 "..\..\Zahlungen.xaml"
            this.ZahlungenDataGrid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.RecordSelected);
            
            #line default
            #line hidden
            
            #line 35 "..\..\Zahlungen.xaml"
            this.ZahlungenDataGrid.AutoGeneratingColumn += new System.EventHandler<System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs>(this.CustomizeHeaders);
            
            #line default
            #line hidden
            return;
            case 3:
            this.NeuZahlungWindow = ((Xceed.Wpf.Toolkit.Primitives.WindowControl)(target));
            
            #line 41 "..\..\Zahlungen.xaml"
            this.NeuZahlungWindow.CloseButtonClicked += new System.Windows.RoutedEventHandler(this.NeuZahlungWindow_CloseButtonClicked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Zahlungsname = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Zahlungsart = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.Bemerkung = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.StatusButton = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            
            #line 75 "..\..\Zahlungen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.speichern_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

