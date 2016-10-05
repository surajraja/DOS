using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ContactMaster.ChildWindows
{
    public partial class running : ChildWindow
    {
        public event EventHandler SubmitClicked;
        public running()
        {
            InitializeComponent();
            this.Closed += new EventHandler(running_Closed);
        }

        void running_Closed(object sender, EventArgs e)
        {
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SubmitClicked(this, new EventArgs());
            this.DialogResult = true;
        }

   
    }
}

