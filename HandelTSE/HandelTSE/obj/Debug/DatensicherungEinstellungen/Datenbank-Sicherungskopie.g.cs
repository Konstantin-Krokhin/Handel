﻿#pragma checksum "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F10F2FA9B6B4274CE1DCFA9358916AC8331B90FA81B38D32E25AFA3F17BC772E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HandelTSE.DatensicherungEinstellungen;
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


namespace HandelTSE.DatensicherungEinstellungen {
    
    
    /// <summary>
    /// Datenbank_Sicherungskopie
    /// </summary>
    public partial class Datenbank_Sicherungskopie : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Datum;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock VerzeichnisTextBlock;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EinstellungenButton;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox DBkomprimieren;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SicherungButton;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BeendenButton;
        
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
            System.Uri resourceLocater = new System.Uri("/HandelTSE;component/datensicherungeinstellungen/datenbank-sicherungskopie.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
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
            this.Datum = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.VerzeichnisTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.EinstellungenButton = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
            this.EinstellungenButton.Click += new System.Windows.RoutedEventHandler(this.EinstellungenButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DBkomprimieren = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.SicherungButton = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
            this.SicherungButton.Click += new System.Windows.RoutedEventHandler(this.SicherungButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BeendenButton = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\DatensicherungEinstellungen\Datenbank-Sicherungskopie.xaml"
            this.BeendenButton.Click += new System.Windows.RoutedEventHandler(this.ProgrammBeenden_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

