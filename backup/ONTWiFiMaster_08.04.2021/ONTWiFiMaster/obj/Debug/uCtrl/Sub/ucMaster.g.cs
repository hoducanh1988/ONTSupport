﻿#pragma checksum "..\..\..\..\uCtrl\Sub\ucMaster.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D4D7A28C16EB990A5F2906C60953131E9072AA8FB42F22E8D73063B6A02C4A3C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ONTWiFiMaster.uCtrl.Sub;
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


namespace ONTWiFiMaster.uCtrl.Sub {
    
    
    /// <summary>
    /// ucMaster
    /// </summary>
    public partial class ucMaster : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 56 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel sp_setting;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbb_Port1;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbb_Port2;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbb_Time;
        
        #line default
        #line hidden
        
        
        #line 183 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel sp_Main;
        
        #line default
        #line hidden
        
        
        #line 221 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgMaster;
        
        #line default
        #line hidden
        
        
        #line 288 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrl_logsystem;
        
        #line default
        #line hidden
        
        
        #line 302 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrl_loguart;
        
        #line default
        #line hidden
        
        
        #line 316 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrl_loginstrument;
        
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
            System.Uri resourceLocater = new System.Uri("/ONTWiFiMaster;component/uctrl/sub/ucmaster.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
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
            case 2:
            this.sp_setting = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.cbb_Port1 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.cbb_Port2 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.cbb_Time = ((System.Windows.Controls.ComboBox)(target));
            
            #line 78 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
            this.cbb_Time.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbb_Time_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.sp_Main = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.dgMaster = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.scrl_logsystem = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 9:
            this.scrl_loguart = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 10:
            this.scrl_loginstrument = ((System.Windows.Controls.ScrollViewer)(target));
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
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 1:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Primitives.ButtonBase.ClickEvent;
            
            #line 46 "..\..\..\..\uCtrl\Sub\ucMaster.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}
