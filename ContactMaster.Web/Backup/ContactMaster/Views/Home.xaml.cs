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
using System.Text;
using System.IO;
using System.Windows.Threading;
using System.Windows.Printing;
using System.Windows.Browser;


namespace ContactMaster
{
    public partial class Home : Page
    {
        EnterpriseService.ContactServiceClient proxy;
        ChildWindows.ContactPiker ctpk;
        ChildWindows.MainPiker mnpk;
        ChildWindows.running rng;
        
        public Home()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitControls();
        }

        private void InitControls()
        {
            proxy = new EnterpriseService.ContactServiceClient();
            proxy.GetContactDepartsCompleted += new EventHandler<EnterpriseService.GetContactDepartsCompletedEventArgs>(proxy_GetContactDepartsCompleted);
            proxy.GetContactDepartsAsync();

            proxy.GetContactTypesCompleted += new EventHandler<EnterpriseService.GetContactTypesCompletedEventArgs>(proxy_GetContactTypesCompleted);
            proxy.GetContactTypesAsync();

            SetVisible(false);
        }

        void proxy_GetContactTypesCompleted(object sender, EnterpriseService.GetContactTypesCompletedEventArgs e)
        {
            ObservableCollection<EnterpriseService.USLIContactType> contactypes = e.Result;
            cboContactType.ItemsSource = contactypes;
            cboContactType.SelectedIndex = 0;
        }

        void proxy_GetContactDepartsCompleted(object sender, EnterpriseService.GetContactDepartsCompletedEventArgs e)
        {
            ObservableCollection<EnterpriseService.USLITeam> depts = e.Result;
            cboDepts.ItemsSource = depts;
            cboDepts.SelectedIndex = 0;
        }

        private void mainTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTab != null)
            StartSearch();
        }
        private void StartSearch()
        {
            if (rng != null) rng.Close();

            if (cboDepts.SelectedIndex < 1)
            {
                SetVisible(false);
            }
            else
            {
                SetVisible(true);

                int icatlog = mainTab.SelectedIndex;
                int iDeptid = Convert.ToInt32(cboDepts.SelectedValue);
                int iUserType = Convert.ToInt32(cboContactType.SelectedValue);
                int iProdType = 0;

                switch (iDeptid)
                {
                    case 105:
                    case 106:
                    case 7:
                    case 1:
                        psTabItem.Visibility = System.Windows.Visibility.Visible;
                        break;
                    default:
                        psTabItem.Visibility = System.Windows.Visibility.Collapsed;
                        if (icatlog == 3) {
                            mainTab.SelectedIndex = 0;
                            icatlog = 0;
                        }
                        break;
                }

                proxy.GetContactViewCompleted += new EventHandler<EnterpriseService.GetContactViewCompletedEventArgs>(proxy_GetContactViewCompleted);
                proxy.GetContactViewAsync(iDeptid, iUserType, icatlog, iProdType);
            }   
        }

        void proxy_GetContactViewCompleted(object sender, EnterpriseService.GetContactViewCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => processContactView(e.Result));
        }

        void processContactView(EnterpriseService.USLIContactView vw)
        {
            switch (mainTab.SelectedIndex)
            {
                case 0:
                    CustomerGrid.ItemsSource = vw.AsignmentDetails;
                    CustomerGrid.SelectedIndex = 0;
                    break;
                case 1:
                    StateGrid.ItemsSource = vw.AsignmentDetails;
                    StateGrid.SelectedIndex = 0;
                    break;
                case 2:
                    ProductGrid.ItemsSource = vw.AsignmentDetails;
                    ProductGrid.SelectedIndex = 0;
                    break;
                case 3:
                    PSGrid.ItemsSource = vw.AsignmentDetails;
                    PSGrid.SelectedIndex = 0;
                    break;
                default:
                    break;
            }

            txDefaultContact.Text = vw.DefaultContactName;
            SetDeleteDefaultVisible();
        }

        private void SetDeleteDefaultVisible()
        {
            if (txDefaultContact.Text.Trim().Length > 0)
            {
                DeleteDefault.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                DeleteDefault.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void SetVisible(bool bVisible)
        {
            if (bVisible)
            {
                mainTab.Visibility = System.Windows.Visibility.Visible;
                btRow.Visibility = System.Windows.Visibility.Visible;
                DefaultContactBlock.Visibility = System.Windows.Visibility.Visible;
                txError.Visibility = System.Windows.Visibility.Collapsed;
                txError.Text = string.Empty;
            }
            else
            {
                mainTab.Visibility = System.Windows.Visibility.Collapsed;
                btRow.Visibility = System.Windows.Visibility.Collapsed;
                DefaultContactBlock.Visibility = System.Windows.Visibility.Collapsed;
                txError.Visibility = System.Windows.Visibility.Visible;
                txError.Text = "Select a department to start.";
            }
            
        }

        private void cboDepts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartSearch();
        }

        private void cboContactType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<EnterpriseService.USLITeam> depts = (ObservableCollection<EnterpriseService.USLITeam>)cboDepts.ItemsSource;

            if (depts == null) return;

            EnterpriseService.USLITeam tm = new EnterpriseService.USLITeam
                {
                    TeamID = 2,
                    TeamName = "COMMERCIAL EXCESS"
                };

            if (Convert.ToInt32(cboContactType.SelectedValue) > 2)
            {
                if (CExExsites() == 1)
                {
                    int i = cboDepts.SelectedIndex;
                    depts.RemoveAt(1);
                    if (i > 1)
                    {
                        cboDepts.SelectedIndex = i-1;
                    }
                    else
                    {
                        cboDepts.SelectedIndex = i;
                    }
                }
            }
            else
            {
                if (CExExsites() == 0)
                {
                    depts.Insert(1, tm);
                }
            }
            StartSearch();
        }

        private int CExExsites()
        {
            int iReturn = cboDepts.SelectedIndex;

            if (iReturn > -1)
            {
                iReturn = 0;
                ObservableCollection<EnterpriseService.USLITeam> depts = (ObservableCollection<EnterpriseService.USLITeam>)cboDepts.ItemsSource;

                foreach (EnterpriseService.USLITeam tm2 in depts)
                {
                    if (tm2.TeamID == 2)
                    {
                        iReturn = 1;
                        break;
                    }
                }
            }


            return iReturn;
        }

        private void ChangeDefault_Click(object sender, RoutedEventArgs e)
        {
            ctpk = new ChildWindows.ContactPiker();
            ctpk.Show();
            ctpk.SubmitClicked += new EventHandler(ctpk_SubmitClicked);
        }

        private void DeleteDefault_Click(object sender, RoutedEventArgs e)
        {
            EnterpriseService.USLIContactIDs cntids = new EnterpriseService.USLIContactIDs
            {
                ContactTypeID = Convert.ToInt32(cboContactType.SelectedValue),
                DeptID = Convert.ToInt32(cboDepts.SelectedValue)
            };

            proxy.DeleteDefaultContactCompleted += new EventHandler<EnterpriseService.DeleteDefaultContactCompletedEventArgs>(proxy_DeleteDefaultContactCompleted);
            proxy.DeleteDefaultContactAsync(cntids);
        }

        void proxy_DeleteDefaultContactCompleted(object sender, EnterpriseService.DeleteDefaultContactCompletedEventArgs e)
        {
            txDefaultContact.Text = string.Empty;
            SetDeleteDefaultVisible();
        }

        void ctpk_SubmitClicked(object sender, EventArgs e)
        {
            txDefaultContact.Text = ctpk.ContactName;

            EnterpriseService.USLIContactIDs cntids = new EnterpriseService.USLIContactIDs
            {
                ContactID = ctpk.ContactId,
                ContactTypeID = Convert.ToInt32(cboContactType.SelectedValue),
                DeptID = Convert.ToInt32(cboDepts.SelectedValue)
            };

            proxy.SaveDefaultContactCompleted += new EventHandler<EnterpriseService.SaveDefaultContactCompletedEventArgs>(proxy_SaveDefaultContactCompleted);
            proxy.SaveDefaultContactAsync(cntids);
        }

        void proxy_SaveDefaultContactCompleted(object sender, EnterpriseService.SaveDefaultContactCompletedEventArgs e)
        {
            SetDeleteDefaultVisible();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            mnpk = new ChildWindows.MainPiker(mainTab.SelectedIndex, Convert.ToInt32(cboDepts.SelectedValue));
            mnpk.Show();
            mnpk.SubmitClicked += new EventHandler(mnpk_SubmitClicked);
        }

        void mnpk_SubmitClicked(object sender, EventArgs e)
        {
            rng = new ChildWindows.running();
            rng.Show();

            string deptid = cboDepts.SelectedValue.ToString();
            string conttype = cboContactType.SelectedValue.ToString();
            int catlog = mainTab.SelectedIndex;

            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.AppendFormat(" \"DeptID\":{0}, \"ContactTypeID\":{1}, \"Catlog\":{2}, \"ContactID\":{3}, ", deptid, conttype, catlog.ToString(), mnpk.ContactID);
            sb.AppendFormat(" \"Entity1ID\":[{0}], \"Entity2ID\":[{1}] ", mnpk.Entity1IDs, mnpk.Entity2IDs);
            sb.Append("}");

            proxy.SaveAssignmentsCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(proxy_SaveAssignmentsCompleted);
            proxy.SaveAssignmentsAsync(sb.ToString());

            rng.SubmitClicked += new EventHandler(rng_SubmitClicked);
            
        }

        void rng_SubmitClicked(object sender, EventArgs e)
        {
            StartSearch();
        }

        void proxy_SaveAssignmentsCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => StartSearch());
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<EnterpriseService.USLIContactIDs> lst = new ObservableCollection<EnterpriseService.USLIContactIDs>();
            int deptid = Convert.ToInt32(cboDepts.SelectedValue);
            int conttype = Convert.ToInt32(cboContactType.SelectedValue);
            int catlog = mainTab.SelectedIndex;

            rng = new ChildWindows.running();
            rng.Show();

            switch (catlog)
            {
                case 0:
                    foreach (EnterpriseService.USLIContactAsignment asmt in CustomerGrid.SelectedItems)
                    {
                        lst.Add(new EnterpriseService.USLIContactIDs
                        {
                            DeptID = deptid,
                            ContactTypeID = conttype,
                            CatlogID = catlog,
                            ContactID = asmt.UserID,
                            EntityID = asmt.Entity1ID,
                            Entity2ID = asmt.Entity2ID
                        });
                    }
                    break;
                case 1:
                    foreach (EnterpriseService.USLIContactAsignment asmt in StateGrid.SelectedItems)
                    {
                        lst.Add(new EnterpriseService.USLIContactIDs
                        {
                            DeptID = deptid,
                            ContactTypeID = conttype,
                            CatlogID = catlog,
                            ContactID = asmt.UserID,
                            EntityID = asmt.Entity1ID,
                            Entity2ID = asmt.Entity2ID
                        });
                    }
                    break;
                case 2:
                    foreach (EnterpriseService.USLIContactAsignment asmt in ProductGrid.SelectedItems)
                    {
                        lst.Add(new EnterpriseService.USLIContactIDs
                        {
                            DeptID = deptid,
                            ContactTypeID = conttype,
                            CatlogID = catlog,
                            ContactID = asmt.UserID,
                            EntityID = asmt.Entity1ID,
                            Entity2ID = asmt.Entity2ID
                        });
                    }
                    break;
                case 3:
                    foreach (EnterpriseService.USLIContactAsignment asmt in PSGrid.SelectedItems)
                    {
                        lst.Add(new EnterpriseService.USLIContactIDs
                        {
                            DeptID = deptid,
                            ContactTypeID = conttype,
                            CatlogID = catlog,
                            ContactID = asmt.UserID,
                            EntityID = asmt.Entity1ID,
                            Entity2ID = asmt.Entity2ID
                        });
                    }
                    break;
                default:
                    break;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat(" \"DeptID\":{0}, \"ContactTypeID\":{1}, \"Catlog\":{2},", deptid.ToString(), conttype.ToString(), catlog.ToString());

            StringBuilder sb1 = new StringBuilder();

            int i = 0;
            foreach (EnterpriseService.USLIContactIDs ctid in lst)
            {
                if (i == 0)
                {
                    sb1.AppendFormat("\"{0}:{1}:{2}\"", ctid.ContactID.ToString(), ctid.EntityID.ToString(), ctid.Entity2ID.ToString());
                    i++;
                }
                else
                {
                    sb1.AppendFormat(",\"{0}:{1}:{2}\"", ctid.ContactID.ToString(), ctid.EntityID.ToString(), ctid.Entity2ID.ToString());
                }

                if (sb1.Length > 8000)
                {
                    StringBuilder sb2 = new StringBuilder();
                    sb2.Append(sb.ToString());
                    sb2.AppendFormat(" \"Entity\":[{0}]", sb1.ToString());
                    sb2.Append("}");

                    i = 0;
                    sb1 = new StringBuilder();

                    DeleteAssignments(sb2.ToString());
                }
            }

            sb.AppendFormat(" \"Entity\":[{0}]", sb1.ToString());
            sb.Append("}");

            DeleteAssignments(sb.ToString());

            rng.SubmitClicked += new EventHandler(rng_SubmitClicked);
        }

        void DeleteAssignments(string delist)
        {
            proxy.DeleteAssignmentsCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(proxy_DeleteAssignmentsCompleted);
            proxy.DeleteAssignmentsAsync(delist);
        }


        void proxy_DeleteAssignmentsCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => StartSearch());
        }

        private void btPrint_Click(object sender, RoutedEventArgs e)
        {
            double headerHeight = 100.0;
            double footerHeight = 30.0;
            double tboxHeight = 20.0;
            int numElements = -1;
            int page = 1;
            int totalPages = 0;
            int counter = 1;
            ObservableCollection<EnterpriseService.USLIContactAsignment> astList = new ObservableCollection<EnterpriseService.USLIContactAsignment>();

            switch (mainTab.SelectedIndex)
            {
                case 0:
                    if (CustomerGrid.ItemsSource == null) return;
                    astList = (ObservableCollection<EnterpriseService.USLIContactAsignment>)CustomerGrid.ItemsSource;
                    break;
                case 1:
                    if (StateGrid.ItemsSource == null) return;
                    astList = (ObservableCollection<EnterpriseService.USLIContactAsignment>)StateGrid.ItemsSource;
                    break;
                case 2:
                    if (ProductGrid.ItemsSource == null) return;
                    astList = (ObservableCollection<EnterpriseService.USLIContactAsignment>)ProductGrid.ItemsSource;
                    break;
                case 3:
                    if (PSGrid.ItemsSource == null) return;
                    astList = (ObservableCollection<EnterpriseService.USLIContactAsignment>)PSGrid.ItemsSource;
                    break;
                default:
                    return;
            }

            PrintDocument pd = new PrintDocument();

            pd.PrintPage += (s, args) =>
            {
                Grid headerGrid = new Grid()
                {
                    Width = args.PrintableArea.Width,
                    Margin = new Thickness(10, 20, 10, 20),
                };

                headerGrid.RowDefinitions.Add(new RowDefinition());

                ColumnDefinition c = new ColumnDefinition();
                c.Width = new GridLength(0, GridUnitType.Auto);
                headerGrid.ColumnDefinitions.Add(c);
                ColumnDefinition b = new ColumnDefinition();
                b.Width = new GridLength(0, GridUnitType.Auto);
                headerGrid.ColumnDefinitions.Add(b);
                ColumnDefinition a = new ColumnDefinition();
                a.Width = new GridLength(0, GridUnitType.Auto);
                headerGrid.ColumnDefinitions.Add(a);

                if (page == 1)
                {
                    numElements = (int)((args.PrintableArea.Height - headerHeight - footerHeight) / tboxHeight);

                    int totalItems = astList.Count;
                    totalPages = (int)Math.Ceiling(totalItems / (double)numElements);
                }

                TextBlock headerDate = new TextBlock()
                {
                    Text = "Page " + page.ToString() + " of " + totalPages.ToString() + "  Printed at: " + DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString(),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 20),
                    FontSize = 12
                };

                headerGrid.RowDefinitions.Add(new RowDefinition());
                headerGrid.Children.Add(headerDate);
                Grid.SetColumn(headerDate, 1);
                Grid.SetRow(headerDate, 0);
                AddPrintHeader(mainTab.SelectedIndex, headerGrid, 1);                

                IEnumerable<EnterpriseService.USLIContactAsignment> ppl = astList.ToList<EnterpriseService.USLIContactAsignment>().Skip((page - 1) * numElements).Take(numElements);

                int i = 2;
                foreach (EnterpriseService.USLIContactAsignment p in ppl)
                {

                    TextBlock ContactBlock = new TextBlock();
                    ContactBlock.Text = p.UserName;
                    ContactBlock.Height = tboxHeight;
                    ContactBlock.Margin = new Thickness(0, 0, 10, 0);
                    
                    TextBlock nameBlock = new TextBlock();
                    nameBlock.Text = p.Entity1Name;
                    nameBlock.Height = tboxHeight;
                    nameBlock.Margin = new Thickness(0, 0, 10, 0);

                    TextBlock name2Block = new TextBlock();
                    if (!(p.Entity2Name == null))
                    {
                        name2Block.Text = p.Entity2Name;
                        name2Block.Height = tboxHeight;
                    }

                    headerGrid.RowDefinitions.Add(new RowDefinition());
                    headerGrid.Children.Add(ContactBlock);
                    headerGrid.Children.Add(nameBlock);
                    headerGrid.Children.Add(name2Block);
                    Grid.SetColumn(ContactBlock, 0);
                    Grid.SetColumn(nameBlock, 1);
                    Grid.SetColumn(name2Block, 2);
                    Grid.SetRow(ContactBlock, i);
                    Grid.SetRow(nameBlock, i);
                    Grid.SetRow(name2Block, i);

                    i++;
                    counter++;

                }
                args.HasMorePages = !(page == totalPages);
                StackPanel dynamicPanel = new StackPanel();
                dynamicPanel.Children.Add(headerGrid);

                args.PageVisual = dynamicPanel;

                page++;

            };

            pd.EndPrint += (s, args) =>
            {
                
            };

            pd.Print("Print Screen");

        }

        private void btExcel_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<EnterpriseService.USLIContactAsignment> astList = new ObservableCollection<EnterpriseService.USLIContactAsignment>();

            string columndata = string.Empty;
            int icatlog = mainTab.SelectedIndex;
            int iDeptid = Convert.ToInt32(cboDepts.SelectedValue);
            int iUserType = Convert.ToInt32(cboContactType.SelectedValue);
            int iProdType = 0;

            StringBuilder sb = new StringBuilder();
            sb.Append("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">Contact Name</Data></Cell>");

            switch (icatlog)
            {
                case 0:
                    if (CustomerGrid.ItemsSource == null) return;
                    astList = (ObservableCollection<EnterpriseService.USLIContactAsignment>)CustomerGrid.ItemsSource;
                    if (astList.Count == 0) return;
                    

                    sb.Append("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">Customer Name</Data></Cell>");
                    columndata = sb.ToString();



                    break;
                case 1:
                    if (StateGrid.ItemsSource == null) return;
                    astList = (ObservableCollection<EnterpriseService.USLIContactAsignment>)StateGrid.ItemsSource;
                    if (astList.Count == 0) return;

                    sb.Append("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">State Name</Data></Cell>");

                    columndata = sb.ToString();

                    break;
                case 2:
                    if (ProductGrid.ItemsSource == null) return;
                    astList = (ObservableCollection<EnterpriseService.USLIContactAsignment>)ProductGrid.ItemsSource;
                    if (astList.Count == 0) return;

                    sb.Append("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">Product Name</Data></Cell>");

                    columndata = sb.ToString();

                    break;
                case 3:
                    if (PSGrid.ItemsSource == null) return;
                    astList = (ObservableCollection<EnterpriseService.USLIContactAsignment>)PSGrid.ItemsSource;
                    if (astList.Count == 0) return;

                    sb.Append("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">Product Name</Data></Cell>");
                    sb.Append("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">State Name</Data></Cell>");

                    columndata = sb.ToString();

                    break;
                default:
                    break;

            }
           
            proxy.ExcelReportCompleted += new EventHandler<EnterpriseService.ExcelReportCompletedEventArgs>(proxy_ExcelReportCompleted);
            proxy.ExcelReportAsync(columndata, iDeptid, iUserType, icatlog, iProdType);
        }

        void proxy_CreateExcelReportCompleted(object sender, EnterpriseService.CreateExcelReportCompletedEventArgs e)
        {
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(e.Result), "_newWindow");
        }

        void proxy_ExcelReportCompleted(object sender, EnterpriseService.ExcelReportCompletedEventArgs e)
        {
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(e.Result), "_newWindow");
        }

        private void cboProds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartSearch();
        }

        private void AddPrintHeader(int tabindex, Grid headerGrid, int rowNumber)
        {
            TextBlock headerContact = new TextBlock()
            {
                Text = "Contact Name",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 6),
                FontSize = 12,
                FontWeight = FontWeights.Bold
            };

            TextBlock headerAgent = new TextBlock()
            {
                Text = "Customer Name",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 6),
                FontSize = 12,
                FontWeight = FontWeights.Bold
            };

            TextBlock headerState = new TextBlock()
            {
                Text = "State Name",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 6),
                FontSize = 12,
                FontWeight = FontWeights.Bold
            };

            TextBlock headerProduct = new TextBlock()
            {
                Text = "Product Name",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 6),
                FontSize = 12,  
                FontWeight = FontWeights.Bold
            };

            switch (tabindex)
            {
                case 0:
                    headerGrid.Children.Add(headerContact);
                    headerGrid.Children.Add(headerAgent);
                    Grid.SetColumn(headerContact, 0);
                    Grid.SetColumn(headerAgent, 1);
                    Grid.SetRow(headerContact, rowNumber);
                    Grid.SetRow(headerAgent, rowNumber);
                    break;
                case 1:
                    headerGrid.Children.Add(headerContact);
                    headerGrid.Children.Add(headerState);
                    Grid.SetColumn(headerContact, 0);
                    Grid.SetColumn(headerState, 1);
                    Grid.SetRow(headerContact, rowNumber);
                    Grid.SetRow(headerState, rowNumber);
                    break;
                case 2:
                    headerGrid.Children.Add(headerContact);
                    headerGrid.Children.Add(headerProduct);
                    Grid.SetColumn(headerContact, 0);
                    Grid.SetColumn(headerProduct, 1);
                    Grid.SetRow(headerContact, rowNumber);
                    Grid.SetRow(headerProduct, rowNumber);
                    break;
                case 3:
                    headerGrid.Children.Add(headerContact);
                    headerGrid.Children.Add(headerProduct);
                    headerGrid.Children.Add(headerState);
                    Grid.SetColumn(headerContact, 0);
                    Grid.SetColumn(headerProduct, 1);
                    Grid.SetColumn(headerState, 2);
                    Grid.SetRow(headerContact, rowNumber);
                    Grid.SetRow(headerProduct, rowNumber);
                    Grid.SetRow(headerState, rowNumber);
                    break;
                default:
                    break;

            }
        }
    }
}