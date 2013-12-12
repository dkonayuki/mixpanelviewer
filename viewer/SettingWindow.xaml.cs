using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace MixPanelViewer
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        #region RoutedEvent
        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent OnAPIChangedEvent = EventManager.RegisterRoutedEvent(
            "OnAPIChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SettingWindow));

        // Provide CLR accessors for the event
        public event RoutedEventHandler OnAPIChanged
        {
            add { AddHandler(OnAPIChangedEvent, value); }
            remove { RemoveHandler(OnAPIChangedEvent, value); }
        }

        // This method raises the Tap event
        void RaiseChangeEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SettingWindow.OnAPIChangedEvent);
            RaiseEvent(newEventArgs);
        }
        #endregion

        public SettingWindow()
        {
            InitializeComponent();
            txt_API_Key.Text = MixPanel.Default.ApiKey;
            txt_API_Secret.Text = MixPanel.Default.ApiSecret;
        }

        private void onOkClick(object sender, RoutedEventArgs e)
        {
            if ((txt_API_Key.Text != MixPanel.Default.ApiKey) || 
                (txt_API_Secret.Text != MixPanel.Default.ApiSecret))
            {
                MixPanel.Default.ApiKey = txt_API_Key.Text;
                MixPanel.Default.ApiSecret = txt_API_Secret.Text;

                Dictionary<string, string> apiList = new Dictionary<string, string>();
                apiList["api_key"] = MixPanel.Default.ApiKey;
                apiList["api_secret"] = MixPanel.Default.ApiSecret;
                string jsonStr = JsonSerializer.SerializeObject(apiList);

                string filePath = Utilities.GetAppDir();
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = Utilities.GetSettingPath();
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(jsonStr);
                    writer.Close();
                }
                /*Add refresh button after changing*/
                RaiseChangeEvent();
            }
            this.Close();
        }

        private void onCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
