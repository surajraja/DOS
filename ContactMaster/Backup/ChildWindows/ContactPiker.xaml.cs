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
    public partial class ContactPiker : ChildWindow
    {
        EnterpriseService.ContactServiceClient proxy;

        public int ContactId { get; set; }
        public string ContactName { get; set; }

        public event EventHandler SubmitClicked;

        public ContactPiker()
        {
            InitializeComponent();

            proxy = new EnterpriseService.ContactServiceClient();
            proxy.GetUsersCompleted += new EventHandler<EnterpriseService.GetUsersCompletedEventArgs>(proxy_GetUsersCompleted);
            proxy.GetUsersAsync();
        }

        void proxy_GetUsersCompleted(object sender, EnterpriseService.GetUsersCompletedEventArgs e)
        {
            ContactGrid.ItemsSource = e.Result;
            ContactGrid.SelectedIndex = 0;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnterpriseService.USLIUsers user = (EnterpriseService.USLIUsers)ContactGrid.SelectedItem;
            ContactId = user.UserID;
            ContactName = user.UserName;

            SubmitClicked(this, new EventArgs());

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

