﻿#pragma checksum "D:\GoShopping\GoShopping\GoShopping\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5468EE5A82999B854A3AD0AA1F397784"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GoShopping.Controls;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace GoShopping {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal GoShopping.Controls.PullDownItem pullDownItemBehind;
        
        internal System.Windows.Controls.ItemsControl todoList;
        
        internal GoShopping.Controls.PullDownItem pullDownItemInFront;
        
        internal GoShopping.Controls.DragImage dragImageControl;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GoShopping;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.pullDownItemBehind = ((GoShopping.Controls.PullDownItem)(this.FindName("pullDownItemBehind")));
            this.todoList = ((System.Windows.Controls.ItemsControl)(this.FindName("todoList")));
            this.pullDownItemInFront = ((GoShopping.Controls.PullDownItem)(this.FindName("pullDownItemInFront")));
            this.dragImageControl = ((GoShopping.Controls.DragImage)(this.FindName("dragImageControl")));
        }
    }
}

