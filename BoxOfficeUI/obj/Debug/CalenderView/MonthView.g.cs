﻿#pragma checksum "..\..\..\CalenderView\MonthView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6402B67B91892913E91B8D7ACCCF0233C78D2F56"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BoxOfficeUI.CalenderView;
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


namespace BoxOfficeUI.CalenderView {
    
    
    /// <summary>
    /// MonthView
    /// </summary>
    public partial class MonthView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\CalenderView\MonthView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image MonthGoPrev;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\CalenderView\MonthView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label MonthYearLabel;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\CalenderView\MonthView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image MonthGoNext;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\CalenderView\MonthView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MonthViewGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/BoxOfficeUI;component/calenderview/monthview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CalenderView\MonthView.xaml"
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
            this.MonthGoPrev = ((System.Windows.Controls.Image)(target));
            
            #line 28 "..\..\..\CalenderView\MonthView.xaml"
            this.MonthGoPrev.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.MonthGoPrev_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MonthYearLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.MonthGoNext = ((System.Windows.Controls.Image)(target));
            
            #line 34 "..\..\..\CalenderView\MonthView.xaml"
            this.MonthGoNext.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.MonthGoNext_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.MonthViewGrid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

