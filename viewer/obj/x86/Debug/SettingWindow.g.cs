﻿#pragma checksum "..\..\..\SettingWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A2F32BF97BB3603060FCBFD4E261030B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace MixPanelViewer {
    
    
    /// <summary>
    /// SettingWindow
    /// </summary>
    public partial class SettingWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\SettingWindow.xaml"
        internal System.Windows.Controls.TextBox txt_API_Key;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\SettingWindow.xaml"
        internal System.Windows.Controls.TextBox txt_API_Secret;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\SettingWindow.xaml"
        internal System.Windows.Controls.Button btn_OK;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\SettingWindow.xaml"
        internal System.Windows.Controls.Button btn_Cancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MixPanelGetter;component/settingwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SettingWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txt_API_Key = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.txt_API_Secret = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.btn_OK = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\SettingWindow.xaml"
            this.btn_OK.Click += new System.Windows.RoutedEventHandler(this.onOkClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Cancel = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\SettingWindow.xaml"
            this.btn_Cancel.Click += new System.Windows.RoutedEventHandler(this.onCancelClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

