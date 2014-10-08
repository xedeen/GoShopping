﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GoShopping.Controls
{
    public partial class PullDownItem : UserControl
    {
        public PullDownItem()
        {
            InitializeComponent();
        }

        public string Text
        {
            set
            {
                pullText.Text = value;
            }
        }

        public double VerticalOffset
        {
            set
            {
                //var trans = LayoutRoot.RenderTransform as TranslateTransform;
                //trans.Y = value;
                this.Margin = new Thickness(0, value, 0, 0);
            }
        }
    }
}
