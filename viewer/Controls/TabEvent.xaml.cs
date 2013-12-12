﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Threading;
using System.ComponentModel;
using System.Windows.Documents;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using System.Globalization;

namespace MixPanelViewer.Controls
{
    public partial class TabEvent : UserControl
    {  
        private const string URI_CONTENT    = "https://data.mixpanel.com/api/2.0/export/"; 

        private List<Report> m_Reports;
        private List<string> m_Headers;
        private MenuItem m_MenuItem;
        private ContextMenu m_ContextMenu;
        private State m_State;
        private List<string> m_PropertyList;
        private List<string> m_FilterList;
        private string[] m_NumberOperations = { ">=", "<=", "<", ">" };
        private GridLength cachedColumnWidth;
        private string m_EventType;

        public string EventType
        {
            get { return m_EventType; }
            set { m_EventType = value; }
        }
       
        public static DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(TabEvent));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static DependencyProperty CanRemoveProperty = DependencyProperty.Register("CanRemove", typeof(bool), typeof(TabEvent));

        public bool CanRemove
        {
            get { return (bool)GetValue(CanRemoveProperty); }
            set { SetValue(CanRemoveProperty, value); }
        }

        public delegate void UpdateHandler(object sender, RoutedEventArgs args);

        public event UpdateHandler HeaderUpdated;
        public event UpdateHandler EventListUpdated;

        public TabEvent(List<object> eventList)
        {
            InitializeComponent();
            m_EventType = "New Tab";
            box_Event.ItemsSource = eventList;
            box_Event.SelectedIndex = 0;
            /*by default, the time zone which mixpanel uses is PST*/
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            date_To.SelectedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            date_From.SelectedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow.AddDays(-7), timeZoneInfo);
            m_PropertyList = new List<string>();
            m_FilterList = new List<string>();
            CanRemove = false;
            /*datacontext for dependencyproperty and routedcommand*/
            this.DataContext = this;
        }

        #region Public methods

        public void SetState(string eventName, string dateFrom, string dateTo, string content)
        {
            m_State = new State();
            m_State.EventType = eventName;
            m_State.DateFrom = DateTime.Parse(dateFrom as string);
            m_State.DateTo = DateTime.Parse(dateTo as string);
            m_State.Content = content;
        }

        public void SetState(string eventName, DateTime dateFrom, DateTime dateTo, string content)
        {
            m_State = new State();
            m_State.EventType = eventName;
            m_State.DateFrom = dateFrom;
            m_State.DateTo = dateTo;
            m_State.Content = content;
        }

        public State GetState()
        {
            if (m_State != null)
            {
                return m_State;
            }
            else
            {
                return new State();
            }
        }

        /*Display saved state to current tab*/
        public void DisplayState()
        {
            m_Reports = new List<Report>();
            m_Reports = Utilities.GetReports(m_State.Content);

            displayReports(m_Reports, true);
            date_From.SelectedDate = m_State.DateFrom;
            date_To.SelectedDate = m_State.DateTo;
            box_Event.SelectedValue = m_State.EventType;
            /*update event type property*/
            EventType = m_Reports[0].Event;
            HeaderUpdated(this, null);
        }

        #endregion

        #region Private methods

        private void disableAllControls()
        {
            this.Opacity = 0.8;
            IsLoading = true;
            list_Reports.IsEnabled = false;
            box_Event.IsEnabled = false;
            btn_View.IsEnabled = false;        
        }

        private void enableAllControls()
        {
            this.Opacity = 1;
            IsLoading = false;
            list_Reports.IsEnabled = true;
            box_Event.IsEnabled = true;
            btn_View.IsEnabled = true;
        }

        private bool validateData()
        {
            string err = string.Empty;
            string dateFrom = date_From.SelectedDate.Value.ToString("yyyy-MM-dd");
            string dateTo = date_To.SelectedDate.Value.ToString("yyyy-MM-dd");
            if (String.IsNullOrEmpty(dateFrom) || (String.IsNullOrEmpty(dateTo)))
            {
                err += "Date must be picked!\n";
            }
            if (box_Event.SelectedIndex == -1)
            {
                err += "Event type is not specified.\n";
            }
            if (date_From.SelectedDate.Value > date_To.SelectedDate.Value)
            {
                err += "date_From must be earlier than date_To.\n";
            }
            if (String.IsNullOrEmpty(err))
            {
                return true;
            }
            else
            {
                MessageBox.Show(err);
                return false;
            }
        } 

        private void clearReportsList()
        {
            list_Reports.Columns.Clear();
            list_Reports.ItemsSource = null;
        }

        private void displayCounter(int count)
        {
            txt_Total.Text = count.ToString();
        }

        private void displayReports(List<Report> Reports, bool isNew)
        {
            m_Headers = Reports[0].Properties.Keys.ToList<string>();
            DataGridTextColumn column = new DataGridTextColumn();

            clearReportsList();

            /*Change No. Column's position*/
            m_Headers.Reverse(0, m_Headers.Count - 1);
            m_Headers.Reverse(0, m_Headers.Count);
            /*Remove unnecessary columns*/
            if (m_Headers.Contains("time"))
            {
                m_Headers.Remove("time");
            }
            if (m_Headers.Contains("distinct_id"))
            {
                m_Headers.Remove("distinct_id");
            }

            /*Add other columns*/
            foreach (string header in m_Headers)
            {
                column = new DataGridTextColumn();
                column.Header = header;
                column.Binding = new Binding("[" + header + "]");
                list_Reports.Columns.Add(column);
            }

            /*Create row binding*/
            List<IDictionary<string, object>> rows = new List<IDictionary<string, object>>();
            foreach (Report report in Reports)
            {
                rows.Add(report.Properties);
            }
            list_Reports.ItemsSource = rows;

            /*Create context menu for hiding/unhiding if necessary*/
            if (isNew)
            {
                createContextMenu();
                /*Update Property list for filter function*/
                m_PropertyList = m_Headers;
                m_PropertyList.Remove("No.");
                m_PropertyList.Sort();
            }

            displayCounter(m_Reports.Count);
        }

        /* user cannot use keyboard on datagrid*/
        private void onPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private bool isUsingFilter()
        {
            return ((panel_Filter.IsExpanded) && (m_FilterList.Count > 0));
        }

        #endregion

        #region ClickEventHandlers

        private void onViewClick(object sender, RoutedEventArgs e)
        {
            string dateFrom = date_From.SelectedDate.Value.ToString("yyyy-MM-dd");
            string dateTo = date_To.SelectedDate.Value.ToString("yyyy-MM-dd"); 
            if (validateData())
            {
                disableAllControls();
                string eventName = box_Event.SelectedItem.ToString();
                string where = "";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                if (isUsingFilter())
                {
                    foreach (string expression in m_FilterList)
                    {
                        string[] temp = expression.Split(new char[] { ' ' });
                        if (temp[1] == "IN")
                        {
                            temp[1] = "in";
                            temp[0] = "\"" + temp[0] + "\"";
                            temp[2] = "properties[\"" + temp[2] + "\"]";
                        }
                        else
                        {
                            /*Convert to number type if necessary*/
                            if (m_NumberOperations.Contains(temp[1]))
                            {
                                temp[0] = "number" + "(" + "properties[\"" + temp[0] + "\"]" + ")";
                                temp[2] = "number" + "(" + "\"" + temp[2] + "\"" + ")";
                            }
                            else
                            {
                                /*Default: string type*/
                                temp[0] = "properties[\"" + temp[0] + "\"]";
                                temp[2] = "\"" + temp[2] + "\"";
                            }  
                        }
                        /*Add all expressions*/
                        if (!string.IsNullOrEmpty(where))
                        {
                            where = where + " and " + "(" + temp[0] + temp[1] + temp[2] + ")";
                        }
                        else
                        {
                            where = "(" + temp[0] + temp[1] + temp[2] + ")";
                        }
                    }
                    parameters.Add("where", where);
                }
                int expireTime = Utilities.ConvertToTimestamp(DateTime.Now.AddMinutes(1));
                parameters.Add("from_date", dateFrom);
                parameters.Add("to_date", dateTo);
                parameters.Add("event", "[\"" + eventName + "\"]");
                parameters.Add("expire", expireTime.ToString());

                string uri = MixPanel.Default.GetUri(parameters, URI_CONTENT);
                   
                MixPanel.Default.HttpGet(uri, (object sender2, DownloadStringCompletedEventArgs e2) =>
                {
                    enableAllControls();
                    if (e2.Error != null)
                    {
                        MessageBox.Show(e2.Error.Message);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(e2.Result))
                        {
                            string list = e2.Result;
                            list = list.Replace('\n', ',');
                            list = '[' + list + ']';
                    
                            SetState(box_Event.SelectedValue as string,
                                date_From.SelectedDate ?? DateTime.Now,
                                date_To.SelectedDate ?? DateTime.Now,
                                list);
                 
                            DisplayState();
                        }
                        else
                        {
                            clearReportsList();
                            displayCounter(0);
                        }
                    }
                });     
            }
        }

        private void onAddClick(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new FilterWindow(m_PropertyList);
            filterWindow.FilterUpdated += filterUpdated;
            filterWindow.ShowDialog();
        }

        private void onRemoveClick(object sender, RoutedEventArgs e)
        {
            int index = list_Filter.SelectedIndex;
            if (index > -1)
            {
                m_FilterList.RemoveAt(index);
                list_Filter.ItemsSource = null;
                list_Filter.ItemsSource = m_FilterList;
            }
        }

        private void onRefreshClick(object sender, RoutedEventArgs e)
        {
            EventListUpdated(this, null);
            btn_Refresh.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void onListFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_Filter.SelectedIndex != -1)
            {
                CanRemove = true;
            }
            else
            {
                CanRemove = false;
            }
        }

        private void onEditClick(object sender, MouseButtonEventArgs e)
        {
            if (list_Filter.SelectedIndex != -1)
            {
                String[] temp = list_Filter.SelectedItem.ToString().Split(new Char[] {' '});
                FilterWindow filterWindow;
                if (temp[1] == "IN")
                {
                    filterWindow = new FilterWindow(m_PropertyList, temp[2], temp[1], temp[0]);
                }
                else
                {
                    filterWindow = new FilterWindow(m_PropertyList, temp[0], temp[1], temp[2]);
                }
                filterWindow.FilterUpdated += filterEdited;
                filterWindow.ShowDialog();
            }
        }

        private void filterUpdated(object sender, FilterUpdateEventArgs e)
        {
            if (e.Operation == "IN")
            {
                m_FilterList.Add(e.Value + " " + e.Operation + " " + e.Property);
            }
            else
            {
                m_FilterList.Add(e.Property + " " + e.Operation + " " + e.Value);
            }
            list_Filter.ItemsSource = null;
            list_Filter.ItemsSource = m_FilterList;
        }

        private void filterEdited(object sender, FilterUpdateEventArgs e)
        {
            int index = list_Filter.SelectedIndex;
            m_FilterList.RemoveAt(index);
            if (e.Operation == "IN")
            {
                m_FilterList.Add(e.Value + " " + e.Operation + " " + e.Property);
            }
            else
            {
                m_FilterList.Add(e.Property + " " + e.Operation + " " + e.Value);
            }
            list_Filter.ItemsSource = null;
            list_Filter.ItemsSource = m_FilterList;
        }

        private void onColapseClick(object sender, MouseButtonEventArgs e)
        {
            GridSplitter splitter = sender as GridSplitter;
            Grid grid = splitter.Parent as Grid;
            //get the first column (0)
            int colIndex = (int)splitter.GetValue(Grid.ColumnProperty);
            GridLength gridWidth = grid.ColumnDefinitions[colIndex].Width;
            GridLength splitterWidth = new GridLength(splitter.Width);
            if (gridWidth == splitterWidth)
            {
                //expand
                grid.ColumnDefinitions[colIndex].Width = cachedColumnWidth;
            }
            else
            {
                //colapse
                cachedColumnWidth = gridWidth;
                grid.ColumnDefinitions[colIndex].Width = splitterWidth;
            }
        }

        private void onDeleteKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                onRemoveClick(null, null);
            }
        }

        #endregion

        #region ContextMenu for Hiding/UnHiding column

        private void createContextMenu()
        {
            m_ContextMenu = new ContextMenu();

            foreach (string header in m_Headers)
            {
                m_MenuItem = new MenuItem();
                m_MenuItem.Header = header;
                m_MenuItem.IsChecked = true;
                
                m_ContextMenu.Items.Add(m_MenuItem);
                m_MenuItem.Click += new RoutedEventHandler(onMenuItemClick);
                m_MenuItem.Checked += new RoutedEventHandler(onMenuItemChecked);
                m_MenuItem.Unchecked += new RoutedEventHandler(onMenuItemUnchecked);
            }    
        }

        private void onMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item.IsChecked)
            {
                item.IsChecked = false;
            }
            else
            {
                item.IsChecked = true;
            }
        }

        private void onMenuItemChecked(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            List<string> menuList = new List<string>();
            menuList.Clear();

            foreach (MenuItem menuItem in m_ContextMenu.Items)
            {
                if (menuItem.IsChecked == false)
                {
                    menuList.Add(menuItem.Header.ToString());
                }
            }
         
            displayReports(m_Reports, false);

            foreach (string menuItem in menuList)
            {
                foreach (DataGridColumn column in list_Reports.Columns)
                {
                    if (column.Header.ToString() == menuItem)
                    {
                        list_Reports.Columns.Remove(column);
                        break;
                    }
                }
            }        
        }

        private void onMenuItemUnchecked(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            foreach (DataGridColumn column in list_Reports.Columns)
            {
                if (column.Header.ToString().Contains(item.Header.ToString()))
                {
                    list_Reports.Columns.Remove(column);
                    break;
                }
            }         
        }

        private void onRightMouseClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject depObj = (DependencyObject)e.OriginalSource;
            while ((depObj != null) && !(depObj is DataGridColumnHeader))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }

            if (depObj == null)
            {
                return;
            }

            if (depObj is DataGridColumnHeader)
            {
                DataGridColumnHeader dgColHeader = depObj as DataGridColumnHeader;
                dgColHeader.ContextMenu = m_ContextMenu;
            }
        }

        #endregion
    }
}
