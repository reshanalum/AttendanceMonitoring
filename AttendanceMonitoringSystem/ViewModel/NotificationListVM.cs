using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class NotificationListVM : NotifyPropertyChanged
    {
        private DashboardVM _dashboardVM;
        private AttendanceMonitoringContext _context;

        public NotificationListVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
            _context = new AttendanceMonitoringContext();
            LoadNotifications();
        }

        // Observable collection for the UI
        private ObservableCollection<NotificationDisplay> _notificationList;
        public ObservableCollection<NotificationDisplay> NotificationList
        {
            get => _notificationList;
            set
            {
                _notificationList = value;
                OnPropertyChanged(nameof(NotificationList));
            }
        }

        // Selected notification
        private NotificationDisplay _selectedNotification;
        public NotificationDisplay SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                _selectedNotification = value;
                OnPropertyChanged(nameof(SelectedNotification));
            }
        }

        // Search text
        private string _notificationSearchText;
        public string NotificationSearchText
        {
            get => _notificationSearchText;
            set
            {
                _notificationSearchText = value;
                OnPropertyChanged(nameof(NotificationSearchText));
                ApplyFilter();
            }
        }

        // Load notifications and generate display messages
        private void LoadNotifications()
        {
            var notifications = _context.Notifications
                .OrderByDescending(n => n.NotificationId)
                .ToList();

            var displayList = notifications.Select(notif =>
            {
                string scannedRFID = notif.Message; // raw scanned RFID
                var student = _context.Students.FirstOrDefault(s => s.RFID == scannedRFID);

                string displayMessage;
                if (student != null)
                {
                    displayMessage = $"{student.FirstName} {student.LastName} with RFID {scannedRFID} has scanned";
                }
                else
                {
                    displayMessage = $"Unassigned RFID {scannedRFID} scanned";
                }

                return new NotificationDisplay
                {
                    Notification = notif,
                    DisplayMessage = displayMessage
                };
            }).ToList();

            NotificationList = new ObservableCollection<NotificationDisplay>(displayList);
        }

        // Filter based on search text
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(NotificationSearchText))
            {
                LoadNotifications();
            }
            else
            {
                var filtered = NotificationList
                    .Where(nd => nd.DisplayMessage.ToLower().Contains(NotificationSearchText.ToLower()))
                    .ToList();

                NotificationList = new ObservableCollection<NotificationDisplay>(filtered);
            }
        }

        // Double-click command
        private ICommand _navigateToDetailsCommand;
        public ICommand NavigateToDetailsCommand
        {
            get
            {
                if (_navigateToDetailsCommand == null)
                {
                    _navigateToDetailsCommand = new RelayCommand(param =>
                    {
                        if (SelectedNotification != null)
                        {
                            var notif = SelectedNotification.Notification;
                            var scannedRFID = notif.Message;
                            var student = _context.Students.FirstOrDefault(s => s.RFID == scannedRFID);

                            if (student != null)
                            {
                                System.Windows.MessageBox.Show($"Student: {student.FirstName} {student.LastName}\nRFID: {student.RFID}");
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("This scan is unassigned. You can assign this RFID to a student here.");
                                // Navigate to assignment view logic here
                            }
                        }
                    });
                }
                return _navigateToDetailsCommand;
            }
        }
    }

    // Wrapper class to hold notification and display message
    public class NotificationDisplay
    {
        public Notification Notification { get; set; }
        public string DisplayMessage { get; set; }
    }
}
