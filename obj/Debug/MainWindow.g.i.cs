﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5D62F32E2DD55C554B5B9845561055A7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
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
using System.Windows.Shell;


namespace Circles {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid OuterContainer;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid InnerContainer;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Circles;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse Indicator;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid SelectionPanel;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label SelectionName;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider SelectionSize;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SelectionAction;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid TopBar;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Toggletracking;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Save;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label SaveAs;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Open;
        
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
            System.Uri resourceLocater = new System.Uri("/Circles;component/mainwindow.xaml", System.UriKind.Relative);
            
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
            
            #line 8 "..\..\MainWindow.xaml"
            ((Circles.MainWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.window_keypress);
            
            #line default
            #line hidden
            
            #line 9 "..\..\MainWindow.xaml"
            ((Circles.MainWindow)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.window_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.OuterContainer = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.InnerContainer = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.Circles = ((System.Windows.Controls.Grid)(target));
            
            #line 21 "..\..\MainWindow.xaml"
            this.Circles.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.container_down);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Indicator = ((System.Windows.Shapes.Ellipse)(target));
            return;
            case 6:
            this.SelectionPanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.SelectionName = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.SelectionSize = ((System.Windows.Controls.Slider)(target));
            
            #line 27 "..\..\MainWindow.xaml"
            this.SelectionSize.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SelectionSize_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.SelectionAction = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\MainWindow.xaml"
            this.SelectionAction.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.selectionAction_textChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.TopBar = ((System.Windows.Controls.Grid)(target));
            
            #line 33 "..\..\MainWindow.xaml"
            this.TopBar.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.TopBar_MouseDown);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 38 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseEnter);
            
            #line default
            #line hidden
            
            #line 38 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseLeave);
            
            #line default
            #line hidden
            
            #line 38 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.newcircle_click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 40 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseEnter);
            
            #line default
            #line hidden
            
            #line 40 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseLeave);
            
            #line default
            #line hidden
            
            #line 40 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.closeButton_click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.Toggletracking = ((System.Windows.Controls.Label)(target));
            
            #line 41 "..\..\MainWindow.xaml"
            this.Toggletracking.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.toggle_tracking);
            
            #line default
            #line hidden
            
            #line 41 "..\..\MainWindow.xaml"
            this.Toggletracking.MouseEnter += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseEnter);
            
            #line default
            #line hidden
            
            #line 41 "..\..\MainWindow.xaml"
            this.Toggletracking.MouseLeave += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseLeave);
            
            #line default
            #line hidden
            return;
            case 14:
            this.Save = ((System.Windows.Controls.Label)(target));
            
            #line 42 "..\..\MainWindow.xaml"
            this.Save.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.save_to_file);
            
            #line default
            #line hidden
            
            #line 42 "..\..\MainWindow.xaml"
            this.Save.MouseEnter += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseEnter);
            
            #line default
            #line hidden
            
            #line 42 "..\..\MainWindow.xaml"
            this.Save.MouseLeave += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseLeave);
            
            #line default
            #line hidden
            return;
            case 15:
            this.SaveAs = ((System.Windows.Controls.Label)(target));
            
            #line 43 "..\..\MainWindow.xaml"
            this.SaveAs.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.save_as);
            
            #line default
            #line hidden
            
            #line 43 "..\..\MainWindow.xaml"
            this.SaveAs.MouseEnter += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseEnter);
            
            #line default
            #line hidden
            
            #line 43 "..\..\MainWindow.xaml"
            this.SaveAs.MouseLeave += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseLeave);
            
            #line default
            #line hidden
            return;
            case 16:
            this.Open = ((System.Windows.Controls.Label)(target));
            
            #line 44 "..\..\MainWindow.xaml"
            this.Open.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.open_file);
            
            #line default
            #line hidden
            
            #line 44 "..\..\MainWindow.xaml"
            this.Open.MouseEnter += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseEnter);
            
            #line default
            #line hidden
            
            #line 44 "..\..\MainWindow.xaml"
            this.Open.MouseLeave += new System.Windows.Input.MouseEventHandler(this.closeButton_mouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

