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
using System.Text;
using System.Collections.ObjectModel;

namespace ContactMaster.ChildWindows
{
    public partial class MainPiker : ChildWindow
    {
        public event EventHandler SubmitClicked;
        EnterpriseService.ContactServiceClient proxy;

        public string Entity1IDs { get; set; }
        public string Entity2IDs { get; set; }
        public string ContactID { get; set; }
        public int catlogid { get; set; }
        
        List<EnterpriseService.USLIContactType> contactlist;
        List<EnterpriseService.USLITeam> EntOneList;
       
        public MainPiker(int ctid, int tmid)
        {
            InitializeComponent();

            catlogid = ctid;
            if (catlogid == 0)
            {
                serchEntityOne.Width = 120;
                serchState.Width = 50;
                serchState.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                serchState.Visibility = System.Windows.Visibility.Collapsed;
            }
            proxy = new EnterpriseService.ContactServiceClient();
            proxy.GetContactMainPikerViewCompleted += new EventHandler<EnterpriseService.GetContactMainPikerViewCompletedEventArgs>(proxy_GetContactMainPikerViewCompleted);
            proxy.GetContactMainPikerViewAsync(catlogid, tmid);
        }

        void proxy_GetContactMainPikerViewCompleted(object sender, EnterpriseService.GetContactMainPikerViewCompletedEventArgs e)
        {
            EnterpriseService.USLIContactMainPikerView vw = e.Result;
            if (vw.ContactList != null)
            {
                contactlist = vw.ContactList.ToList();
                ContactGrid.ItemsSource = contactlist;
                ContactGrid.SelectedIndex = 0;
            } 

            if (vw.EntityListOne != null)
            {
                EntOneList = vw.EntityListOne.ToList();
                Entity1Grid.ItemsSource = EntOneList;
                Entity1Grid.SelectedIndex = 0;

                string s = "Select customer(s) from the list";
                switch (catlogid)
                {
                    case 1:
                    case 3:
                        s = "Select state(s) from the list";
                        break;
                    case 2:
                        s = "Select product(s) from the list";
                        break;
                    default:
                        break;
                }
                txE1Name.Text = s;
            }

            if (vw.EntityListTwo != null)
            {
                txE2Name.Visibility = System.Windows.Visibility.Visible;
                Entity2Grid.Visibility = System.Windows.Visibility.Visible;
                Entity2Grid.ItemsSource = vw.EntityListTwo;
                Entity2Grid.SelectedIndex = 0;
                this.Height = 510;
            }

            if (vw.BState != null)
            {
                serchState.ItemsSource = vw.BState;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnterpriseService.USLIContactType contact = (EnterpriseService.USLIContactType)ContactGrid.SelectedItem;
            ContactID = contact.ContactTypeID.ToString();
            Entity2IDs = "0";

            StringBuilder sb = new StringBuilder();
            int i = 0;

            foreach (EnterpriseService.USLITeam tm in Entity1Grid.SelectedItems)
            {
                if (i == 0)
                {
                    sb.Append(tm.TeamID.ToString());
                }
                else
                {
                    sb.Append("," + tm.TeamID.ToString());
                }
                i++;
            }
            Entity1IDs = sb.ToString();

            if (catlogid == 3)
            {
                sb = new StringBuilder();
                i = 0;

                foreach (EnterpriseService.USLITeam prodtm in Entity2Grid.SelectedItems)
                {
                    if (i == 0)
                    {
                        sb.Append(prodtm.TeamID.ToString());
                    }
                    else
                    {
                        sb.Append("," + prodtm.TeamID.ToString());
                    }
                    i++;
                }
                Entity2IDs = sb.ToString();
            }

            SubmitClicked(this, new EventArgs());
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void serchContact_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = serchContact.Text.Trim();
            if (s.Length > 1)
            {
                s = s.ToUpper();
                ContactGrid.ItemsSource = contactlist.Where(c => c.ContactTypeName.ToUpper().Contains(s));
            }
            else
            {
                ContactGrid.ItemsSource = contactlist;
            }
            ContactGrid.SelectedIndex = 0;
        }

        private void serchEntityOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = serchEntityOne.Text.Trim();
            if (s.Length > 0)
            {
                s = s.ToUpper();
                Entity1Grid.ItemsSource = EntOneList.Where(c => c.TeamName.ToUpper().Contains(s));
            }
            else
            {
                Entity1Grid.ItemsSource = EntOneList;
            }
            Entity1Grid.SelectedIndex = 0;
        }

        private void serchState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string s = serchState.SelectedValue.ToString();
            if (s.Length > 0)
            {
               Entity1Grid.ItemsSource = EntOneList.Where(c => c.TeamState.Contains(s));
            }
            else
            {
                Entity1Grid.ItemsSource = EntOneList;
            }
        }
    }
}

