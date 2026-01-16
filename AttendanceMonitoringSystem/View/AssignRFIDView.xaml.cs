using AttendanceMonitoringSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AttendanceMonitoringSystem.View
{
    /// <summary>
    /// Interaction logic for AssignRFIDView.xaml
    /// </summary>
    public partial class AssignRFIDView : UserControl
    {
        private AssignRFIDVM _vm;
        public AssignRFIDView()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;
        }
        private void ComboBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {

                if (e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Escape ||
                    e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Left || e.Key == Key.Right)
                {
                    return;
                }

        
                bool hasText = !string.IsNullOrEmpty(comboBox.Text);
                bool hasItems = comboBox.Items.Count > 0;

                if (hasText && hasItems)
                {
                    comboBox.IsDropDownOpen = true;

            
                    var textBox = (TextBox)comboBox.Template.FindName("PART_EditableTextBox", comboBox);
                    if (textBox != null)
                    {
                        textBox.SelectionStart = textBox.Text.Length;
                        textBox.SelectionLength = 0;
                    }
                }
                else
                {
         
                    comboBox.IsDropDownOpen = false;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = (AssignRFIDVM)DataContext;
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.SaveCommand(); 
            }
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.BackToNotificationList();
            }
        }
    }
}
