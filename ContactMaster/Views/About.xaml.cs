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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace ContactMaster
{
    public partial class About : Page
    {
        EnterpriseService.ContactServiceClient proxy;
        ObservableCollection<EnterpriseService.USLIUsers> users;
        ChildWindows.running rng;
        ChildWindows.ContactPiker ctpk;
        
        public About()
        {
            InitializeComponent();

            proxy = new EnterpriseService.ContactServiceClient();
            proxy.GetUsersCompleted += new EventHandler<EnterpriseService.GetUsersCompletedEventArgs>(proxy_GetUsersCompleted);
            proxy.GetUsersAsync();
        }

        void proxy_GetUsersCompleted(object sender, EnterpriseService.GetUsersCompletedEventArgs e)
        {
            users = e.Result;
            ContactGrid.ItemsSource = users;
            ContactGrid.SelectedIndex = 0;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            proxy.SearchUsersCompleted += new EventHandler<EnterpriseService.SearchUsersCompletedEventArgs>(proxy_SearchUsersCompleted);
            proxy.SearchUsersAsync(txSearch.Text);
        }

        void proxy_SearchUsersCompleted(object sender, EnterpriseService.SearchUsersCompletedEventArgs e)
        {
            if (e.Result.Count > 0)
            {
                ContactGrid.ItemsSource = e.Result;
            }
            else
            {
                ContactGrid.ItemsSource = users;
            }

            ContactGrid.SelectedIndex = 0;
        }

        private void txSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Enter")
            {
                proxy.SearchUsersCompleted += new System.EventHandler<EnterpriseService.SearchUsersCompletedEventArgs>(proxy_SearchUsersCompleted);
                proxy.SearchUsersAsync(txSearch.Text);
            }
        }

        private void ContactGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnterpriseService.USLIUsers ur = (EnterpriseService.USLIUsers)ContactGrid.SelectedItem;

            proxy.GetUserCompleted += new EventHandler<EnterpriseService.GetUserCompletedEventArgs>(proxy_GetUserCompleted);
            proxy.GetUserAsync(ur.UserID, ur.WorkstationID);
        }

        void proxy_GetUserCompleted(object sender, EnterpriseService.GetUserCompletedEventArgs e)
        {
            EnterpriseService.USLIUser user = e.Result;
            string sUrl = @"http://customers.usli.com/i/USLIteam/" + user.UserImage + ".jpg";

            imageuser.Source = new BitmapImage(new Uri(sUrl, UriKind.Absolute));

            string s = user.FirstName + " " + user.LastName;
            if (user.Title.Length > 0) s += "\r\n" + user.Title;
            if (user.Certification.Length > 0) s += "\r\n" + user.Certification;
            userdetail1.Text = s;

            s = "Department: " + user.Department;
            s += "\r\nPhone Ext: " + user.PhoneExtension;
            s += "\r\nEmail: " + user.Email;
            userdetail2.Text = s;

            GetDetailedView(user.UserID);
        }

        void GetDetailedView(int userId)
        {
            proxy.GetContactDetailViewCompleted += new EventHandler<EnterpriseService.GetContactDetailViewCompletedEventArgs>(proxy_GetContactDetailViewCompleted);
            proxy.GetContactDetailViewAsync(userId);
        }

        void proxy_GetContactDetailViewCompleted(object sender, EnterpriseService.GetContactDetailViewCompletedEventArgs e)
        {
            bool showreplace = false;
            EnterpriseService.USLIContactAsignmentDetailView vw = e.Result;
            if (vw.CustomerList.Count > 0)
            {
                CustomerPanel.Visibility = System.Windows.Visibility.Visible;
                CustomerGrid.ItemsSource = vw.CustomerList;
                showreplace = true;
            }
            else
            {
                CustomerPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (vw.StateList.Count > 0)
            {
                StatePanel.Visibility = System.Windows.Visibility.Visible;
                StateGrid.ItemsSource = vw.StateList;
                showreplace = true;
            }
            else
            {
                StatePanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (vw.ProductList.Count > 0)
            {
                ProductPanel.Visibility = System.Windows.Visibility.Visible;
                ProductGrid.ItemsSource = vw.ProductList;
                showreplace = true;
            }
            else
            {
                ProductPanel.Visibility = System.Windows.Visibility.Collapsed;
            }


            if (vw.CustProdList.Count > 0)
            {
                CustProdPanel.Visibility = System.Windows.Visibility.Visible;
                CustProdGrid.ItemsSource = vw.CustProdList;
                showreplace = true;
            }
            else
            {
                CustProdPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (showreplace)
            {
                ReplaceCust.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ReplaceCust.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private int GetContactId()
        {
            EnterpriseService.USLIUsers cntk = (EnterpriseService.USLIUsers)ContactGrid.SelectedItem;
            return cntk.UserID;
        }

        private void delCust_Click(object sender, RoutedEventArgs e)
        {
            int contactid = GetContactId();
            ObservableCollection<EnterpriseService.USLIContactIDs> lst = new ObservableCollection<EnterpriseService.USLIContactIDs>();

            foreach (EnterpriseService.USLIContactAsignmentDetail dtl in CustomerGrid.SelectedItems)
            {
                lst.Add(new EnterpriseService.USLIContactIDs{
                    DeptID = dtl.ProductLineID,
                    ContactTypeID = dtl.UserTypeID,
                    CatlogID = 0,
                    ContactID = contactid,
                    EntityID = dtl.Entity1ID,
                    Entity2ID = 0
                });
            }
            ProcessDeletion(lst);
        }
        private void delState_Click(object sender, RoutedEventArgs e)
        {
            int contactid = GetContactId();
            ObservableCollection<EnterpriseService.USLIContactIDs> lst = new ObservableCollection<EnterpriseService.USLIContactIDs>();

            foreach (EnterpriseService.USLIContactAsignmentDetail dtl in StateGrid.SelectedItems)
            {
                lst.Add(new EnterpriseService.USLIContactIDs
                {
                    DeptID = dtl.ProductLineID,
                    ContactTypeID = dtl.UserTypeID,
                    CatlogID = 1,
                    ContactID = contactid,
                    EntityID = dtl.Entity1ID,
                    Entity2ID = 0
                });
            }
            ProcessDeletion(lst);
        }
        private void delProd_Click(object sender, RoutedEventArgs e)
        {
            int contactid = GetContactId();
            ObservableCollection<EnterpriseService.USLIContactIDs> lst = new ObservableCollection<EnterpriseService.USLIContactIDs>();

            foreach (EnterpriseService.USLIContactAsignmentDetail dtl in ProductGrid.SelectedItems)
            {
                lst.Add(new EnterpriseService.USLIContactIDs
                {
                    DeptID = dtl.ProductLineID,
                    ContactTypeID = dtl.UserTypeID,
                    CatlogID = 2,
                    ContactID = contactid,
                    EntityID = dtl.Entity1ID,
                    Entity2ID = 0
                });
            }
            ProcessDeletion(lst);
        }
        private void delCustProd_Click(object sender, RoutedEventArgs e)
        {
            int contactid = GetContactId();
            ObservableCollection<EnterpriseService.USLIContactIDs> lst = new ObservableCollection<EnterpriseService.USLIContactIDs>();

            foreach (EnterpriseService.USLIContactAsignmentDetail dtl in CustProdGrid.SelectedItems)
            {
                lst.Add(new EnterpriseService.USLIContactIDs
                {
                    DeptID = dtl.ProductLineID,
                    ContactTypeID = dtl.UserTypeID,
                    CatlogID = 3,
                    ContactID = contactid,
                    EntityID = dtl.Entity2ID,
                    Entity2ID = dtl.Entity1ID
                });
            }
            ProcessDeletion(lst);
        }
        void ProcessDeletion(ObservableCollection<EnterpriseService.USLIContactIDs> lst)
        {
            rng = new ChildWindows.running();
            rng.Show();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int i = 0;

            foreach (EnterpriseService.USLIContactIDs ids in lst)
            {
                if (i > 0)
                {
                    sb.Append(",{");
                }
                else
                {
                    sb.Append("{");
                    i++;
                }

                sb.AppendFormat("\"DeptID\":{0}, ", ids.DeptID.ToString());
                sb.AppendFormat("\"ContactTypeID\":{0}, ", ids.ContactTypeID.ToString());
                sb.AppendFormat("\"Catlog\":{0}, ", ids.CatlogID.ToString());
                sb.AppendFormat("\"ContactID\":{0}, ", ids.ContactID.ToString());
                sb.AppendFormat("\"Entity1ID\":{0}, ", ids.EntityID.ToString());
                sb.AppendFormat("\"Entity2ID\":{0} ", ids.Entity2ID.ToString());
                sb.Append("}");

                if (sb.Length > 8000)
                {
                    StartDelete(sb.ToString());
                    sb = new System.Text.StringBuilder();
                    i = 0;
                }
            }
            StartDelete(sb.ToString());

            rng.SubmitClicked += new EventHandler(rng_SubmitClicked);
        }

        void rng_SubmitClicked(object sender, EventArgs e)
        {
            LoadUserDetail();
        }
        void StartDelete(string sDeleteList)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}]", sDeleteList);

            proxy.DeleteContactAssignmentsCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(proxy_DeleteContactAssignmentsCompleted);
            proxy.DeleteContactAssignmentsAsync(sb.ToString());
        }

        void proxy_DeleteContactAssignmentsCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => LoadUserDetail());
        }

        void LoadUserDetail()
        {
            if (rng != null) rng.Close();

            int contactid = GetContactId();
            GetDetailedView(contactid);
        }

        void ReplaceCust_Click(object sender, EventArgs e)
        {
            ctpk = new ChildWindows.ContactPiker();
            ctpk.Show();

            ctpk.SubmitClicked += new EventHandler(ctpk_SubmitClicked);
        }

        void ctpk_SubmitClicked(object sender, EventArgs e)
        {
            int targetid = ctpk.ContactId;
            int sourceid = GetContactId();

            proxy.ReplaceContactAssignmentsCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(proxy_ReplaceContactAssignmentsCompleted);
            proxy.ReplaceContactAssignmentsAsync(sourceid, targetid);
        }

        void proxy_ReplaceContactAssignmentsCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            hideAllAssignments();
        }

        void hideAllAssignments()
        {
            CustomerPanel.Visibility = System.Windows.Visibility.Collapsed;
            StatePanel.Visibility = System.Windows.Visibility.Collapsed;
            ProductPanel.Visibility = System.Windows.Visibility.Collapsed;
            CustProdPanel.Visibility = System.Windows.Visibility.Collapsed;
            ReplaceCust.Visibility = System.Windows.Visibility.Collapsed;
        }
       
    }
}