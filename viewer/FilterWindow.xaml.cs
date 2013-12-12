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

namespace MixPanelViewer
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public static DependencyProperty IsValidatedProperty = DependencyProperty.Register("IsValidated", typeof(bool), typeof(FilterWindow));

        public bool IsValidated
        {
            get { return (bool)GetValue(IsValidatedProperty); }
            set { SetValue(IsValidatedProperty, value); }
        }

        //implement for passing parameters between windows
        public delegate void FilterUpdateHandler(object sender, FilterUpdateEventArgs e);

        public event FilterUpdateHandler FilterUpdated;

        public FilterWindow(List<string> propertyList)
        {
            InitializeComponent();
            if (propertyList.Count > 0) box_Properties.ItemsSource = propertyList;
            box_Operations.Items.Add("==");
            box_Operations.Items.Add("!=");
            box_Operations.Items.Add(">=");
            box_Operations.Items.Add("<=");
            box_Operations.Items.Add(">");
            box_Operations.Items.Add("<");
            box_Operations.Items.Add("IN");
            IsValidated = false;
            this.DataContext = this;
        }

        public FilterWindow(List<string> propertyList, string property, string operation, string value)
        {
            InitializeComponent();
            if (propertyList.Count > 0) box_Properties.ItemsSource = propertyList;
            box_Operations.Items.Add("==");
            box_Operations.Items.Add("!=");
            box_Operations.Items.Add(">=");
            box_Operations.Items.Add("<=");
            box_Operations.Items.Add(">");
            box_Operations.Items.Add("<");
            box_Operations.Items.Add("IN");
            box_Properties.Text = property;
            box_Operations.Text = operation;
            txt_Value.Text = value;
            IsValidated = false;
            this.DataContext = this;
        }

        private bool validateData()
        {
            if (string.IsNullOrEmpty(box_Properties.Text)) return false;
            if (box_Operations.SelectedIndex == -1) return false;
            if (string.IsNullOrEmpty(txt_Value.Text)) return false;
            return true;
        }

        private void onOkClick(object sender, RoutedEventArgs e)
        {
            string property = box_Properties.Text;
            string operation = box_Operations.Text;
            string value = txt_Value.Text;
            // instance the event args and pass it each value
            FilterUpdateEventArgs args = new FilterUpdateEventArgs(property, operation, value);
            // raise the event with the updated arguments
            FilterUpdated(this, args); 
            this.Close();
        }

        private void onCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void onValueChanged(object sender, TextChangedEventArgs e)
        {
            if (validateData())
            {
                IsValidated = true;
            }
            else
            {
                IsValidated = false;
            }
        }

        private void onOperationChanged(object sender, SelectionChangedEventArgs e)
        {
            if (validateData())
            {
                IsValidated = true;
            }
            else
            {
                IsValidated = false;
            }
        }

        private void onPropertyChanged(object sender, KeyEventArgs e)
        {
            if (validateData())
            {
                IsValidated = true;
            }
            else
            {
                IsValidated = false;
            }
        }

        private void onPropertyKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (validateData())
            {
                IsValidated = true;
            }
            else
            {
                IsValidated = false;
            }
        }
    }
}
