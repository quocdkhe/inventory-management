﻿#pragma checksum "..\..\..\RegisterWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5DE7987EC802E785A238D01B09BD2DFC78B6ECED"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using InventoryManagement;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace InventoryManagement {
    
    
    /// <summary>
    /// RegisterWindow
    /// </summary>
    public partial class RegisterWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\RegisterWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal InventoryManagement.RegisterWindow reigsterWindow;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/InventoryManagement;component/registerwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\RegisterWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.reigsterWindow = ((InventoryManagement.RegisterWindow)(target));
            return;
            case 2:
            
            #line 43 "..\..\..\RegisterWindow.xaml"
            ((System.Windows.Controls.PasswordBox)(target)).PasswordChanged += new System.Windows.RoutedEventHandler(this.OnChangePassword);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 53 "..\..\..\RegisterWindow.xaml"
            ((System.Windows.Controls.PasswordBox)(target)).PasswordChanged += new System.Windows.RoutedEventHandler(this.OnChangeConfirmPassword);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

