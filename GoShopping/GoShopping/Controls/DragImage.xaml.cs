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
    public partial class DragImage : UserControl
    {
        public DragImage()
        {
            InitializeComponent();
        }

        public Image Image
        {
            get { return dragImage; }
        }
    }
}
