﻿#pragma checksum "..\..\..\SubWindow\wifiTestingConfigWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6382E09C534C509C3B602A750FD2B6C2C1BA2CCB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GW040Hv2Project.SubWindow;
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


namespace GW040Hv2Project.SubWindow {
    
    
    /// <summary>
    /// wifiTestingConfigWindow
    /// </summary>
    public partial class wifiTestingConfigWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgtxTestWifi;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn txtFrequency;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn txtAntena;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn txtAttenuator;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn txtUpperLimit;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn txtLowerLimit;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Uri resourceLocater = new System.Uri("/GW040Hv2;component/subwindow/wifitestingconfigwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
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
            
            #line 10 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 22 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Label_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.dgtxTestWifi = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 4:
            this.txtFrequency = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 5:
            this.txtAntena = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 6:
            this.txtAttenuator = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 7:
            this.txtUpperLimit = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 8:
            this.txtLowerLimit = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 9:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\SubWindow\wifiTestingConfigWindow.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
