using System;
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
using MixPanelViewer.Controls;
using Wpf.Controls;
using System.Collections.ObjectModel;

namespace MixPanelViewer
{
    public partial class MainWindow : Window
    {
        private const string URI_EVENTLIST = "http://mixpanel.com/api/2.0/events/names/";

        private ObservableCollection<MyObject> m_TabList;
        private List<object> m_EventList;

        public static DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(MainWindow));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
       
        public MainWindow()
        {
            InitializeComponent();
            tab_List.NewTabItem += 
                delegate(object sender, NewTabItemEventArgs e)
                {
                    // return a new MyObject to be used as the content of the new tabItem
                    MyObject myObject = new MyObject();
                    e.Content = myObject;
                };
        }

        public void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            createMenuCommands();
            m_TabList = new ObservableCollection<MyObject>();    
            createEventList(true);
            /*datacontext for dependencyproperty and routedcommand*/
            this.DataContext = this;
        }

        #region Menu Commands

        private static RoutedCommand menuOpenCommand = new RoutedCommand();
        private static RoutedCommand menuSaveCommand = new RoutedCommand();
        private static RoutedCommand menuNewTabCommand = new RoutedCommand();

        private void createMenuCommands()
        {
            menuSaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            menuOpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            menuNewTabCommand.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(menuSaveCommand, onSaveMenuCommandExecuted));
            CommandBindings.Add(new CommandBinding(menuOpenCommand, onOpenMenuCommandExecuted));
            CommandBindings.Add(new CommandBinding(menuNewTabCommand, onNewTabMenuCommandExecuted));
        }

        private void onOpenMenuCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            onOpenClick(null, null);
        }

        private void onSaveMenuCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {         
            onSaveClick(null, null);
        }

        private void onNewTabMenuCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            onNewTabClick(null, null);
        }

        #endregion

        #region Private Methods

        private void headerUpdated(object sender, RoutedEventArgs args)
        {
            int index = tab_List.SelectedIndex;
            tab_List.ItemsSource = null;
            tab_List.ItemsSource = m_TabList;
            if (index != -1)
            {
                tab_List.SelectedIndex = index;
            }
            else
            {
                tab_List.SelectedIndex = 0;
            }
        }

        private void createEventList(bool isNew)
        {     
            int expireTime = Utilities.ConvertToTimestamp(DateTime.Now.AddMinutes(1));
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type", "general");
            parameters.Add("expire", expireTime.ToString());
            IsLoading = true;

            string uri = MixPanel.Default.GetUri(parameters, URI_EVENTLIST);

            MixPanel.Default.HttpGet(uri, (object sender, DownloadStringCompletedEventArgs e) =>
            {
                IsLoading = false;
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                }
                else
                {
                    m_EventList = Utilities.GetEventsName(e.Result);
                    if (isNew)
                    {
                        /*First time run*/
                        onNewTabClick(null, null);
                    }
                    else
                    {
                        /*Update all tabs' event list*/
                        foreach (MyObject obj in m_TabList)
                        {
                            TabEvent tab = obj.Value as TabEvent;
                            tab.box_Event.ItemsSource = m_EventList;
                            tab.btn_Refresh.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            });
        }

        private void addRefreshButtons(object sender, RoutedEventArgs e)
        {
            foreach (MyObject obj in m_TabList)
            {
                TabEvent tab = obj.Value as TabEvent;
                tab.btn_Refresh.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void eventListUpdated(object sender, RoutedEventArgs e)
        {
            createEventList(false); 
        }

        #endregion

        #region Menu Click Handlers

        private void onSaveClick(object sender, RoutedEventArgs e)
        {
             TabEvent currentTab = tab_List.SelectedItem as TabEvent;
             if (string.IsNullOrEmpty(currentTab.GetState().Content))
             {
                 MessageBox.Show("No data to save.");
             }
             else
             {           
                 SaveFileDialog saveDialog = new SaveFileDialog();
                 saveDialog.DefaultExt = "txt";
                 saveDialog.AddExtension = true;
                 saveDialog.FileName = currentTab.EventType + DateTime.Today.ToString("ddMMyyyy");
                 saveDialog.InitialDirectory = @"C:\";
                 saveDialog.OverwritePrompt = true;
                 saveDialog.Title = "Save";

                 if (saveDialog.ShowDialog().Value)
                 {
                     Dictionary<string, object> saveState = new Dictionary<string, object>();
                     State state = currentTab.GetState();
                     saveState["event_type"] = state.EventType;
                     saveState["date_from"] = state.DateFrom;
                     saveState["date_to"] = state.DateTo;
                     saveState["content"] = state.Content as string;
                     string jsonStr = JsonSerializer.SerializeObject(saveState);

                     using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                     {
                         writer.Write(jsonStr);
                         writer.Close();
                     }
                 }
             }
        }

        private void onOpenClick(object sender, RoutedEventArgs e)
        {
            TabEvent currentTab = tab_List.SelectedItem as TabEvent;
            if (currentTab == null)
            {
                onNewTabClick(null, null);
                currentTab = tab_List.SelectedItem as TabEvent;
            }
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open";
            openDialog.DefaultExt = "txt";
            if (openDialog.ShowDialog().Value)
            {
                using (StreamReader reader = new StreamReader(openDialog.FileName))
                {             
                    string jsonStr = reader.ReadToEnd();
                    IDictionary<string, object> state = JsonSerializer.DeserializeObject(jsonStr) as IDictionary<string, object>;
                    currentTab.SetState(state["event_type"] as string,
                        state["date_from"] as string,
                        state["date_to"] as string,
                        state["content"] as string);

                    currentTab.DisplayState();
                    reader.Close();
                }
            }
        }

        private void onAboutClick(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.ShowDialog();
        }

        private void onSettingClick(object sender, RoutedEventArgs e)
        {
            SettingWindow window = new SettingWindow();
            window.OnAPIChanged += addRefreshButtons;
            window.ShowDialog();
        }

        private void onExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void onNewTabClick(object sender, RoutedEventArgs e)
        {
            TabEvent tab = new TabEvent(m_EventList);
            m_TabList.Add(
                new MyObject {Header = tab.EventType, Value = tab});
            tab.HeaderUpdated += headerUpdated;
            tab.EventListUpdated += eventListUpdated;
            tab_List.ItemsSource = null;
            tab_List.ItemsSource = m_TabList;
            tab_List.SelectedIndex = m_TabList.Count - 1;
        }

        #endregion

    }

    /// <summary>
    ///      a simple class to use as the ItemsSource for the tabcontrol
    /// </summary>
    public class MyObject : INotifyPropertyChanged
    {
        private string _header;
        private TabEvent _value;

        /// <summary>
        ///     Header Property 
        /// </summary>
        public string Header
        {
            get { return _header; }
            set
            {
                if (_header != value)
                {
                    _header = value;
                    OnPropertyChanged("Header");
                }
            }
        }

        /// <summary>
        ///     Value Property 
        /// </summary>
        public TabEvent Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null) propertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
