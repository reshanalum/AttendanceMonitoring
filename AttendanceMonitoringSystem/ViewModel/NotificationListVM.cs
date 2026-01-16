using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using static AttendanceMonitoringSystem.ViewModel.NotificationListVM;

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
            CurrentPage = 1;

            //reloads
            LoadNotifications();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, e) =>
            {
                if (SelectedNotification == null &&
                    string.IsNullOrWhiteSpace(NotificationSearchText))
                {
                    LoadNotifications();
                }
            };
            timer.Start();

            //Pagination
            NextPageCommand = new RelayCommand(_ => GoToPage(CurrentPage + 1), _ => CurrentPage < TotalPages);
            PrevPageCommand = new RelayCommand(_ => GoToPage(CurrentPage - 1));
            GoToPageCommand = new RelayCommand(page => GoToPage((int)page));

            UpdatePagination();

        }

        public ObservableCollection<NotificationDisplay> NotificationList { get; set; }
    = new ObservableCollection<NotificationDisplay>();

        public ObservableCollection<NotificationDisplay> PagedNotifications { get; set; }
            = new ObservableCollection<NotificationDisplay>();


       
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
        private List<NotificationDisplay> _allNotifications;

        private void LoadNotifications()
        {
            var notifications = _context.Notifications
                .OrderByDescending(n => n.NotificationId)
                .ToList();

            _allNotifications = notifications.Select(notif =>
            {
                var student = _context.Students.FirstOrDefault(s => s.RFID == notif.Message);

                return new NotificationDisplay
                {
                    Notification = notif,
                    DisplayMessage = student != null
                        ? $"{student.FirstName} {student.LastName} with RFID {notif.Message} has scanned"
                        : $"Unassigned RFID {notif.Message} scanned"
                };
            }).ToList();

            NotificationList.Clear();
            foreach (var n in _allNotifications)
                NotificationList.Add(n);

            CurrentPage = 1;
            UpdatePagination();
        }


        // Filter based on search text
        private void ApplyFilter()
        {
            LoadNotifications();

            if (string.IsNullOrWhiteSpace(NotificationSearchText))
                return;

            var search = NotificationSearchText.Trim().ToLower();

            var filtered = _allNotifications
                .Where(n => n.DisplayMessage.ToLower().Contains(search))
                .ToList();

            NotificationList.Clear();
            foreach (var n in filtered)
                NotificationList.Add(n);

            CurrentPage = 1;
            UpdatePagination();
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
                            string rfid = SelectedNotification.Notification.Message;

                            // 1. Create the VM and pass the Scanned RFID
                            // 2. Switch the View in the Dashboard
                            // (Assuming your AssignRFIDView is linked to AssignRFIDViewModel in DataTemplates)

                            var assignView = new AssignRFIDView();
                            assignView.DataContext = new AssignRFIDVM(_dashboardVM, rfid);
                            _dashboardVM.CurrentView = assignView;
                        }
                    });
                }
                return _navigateToDetailsCommand;
            }
        }



        // Wrapper class to hold notification and display message
        public class NotificationDisplay
        {
            public Notification Notification { get; set; }
            public string DisplayMessage { get; set; }
        }

   


        //pagination
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                LoadPage();
                UpdatePageButtons();
            }
        }


        private void UpdatePageButtons()
        {
            PageButtons.Clear();

            int start = Math.Max(1, CurrentPage - 2);
            int end = Math.Min(TotalPages, CurrentPage + 2);

            for (int i = start; i <= end; i++)
            {
                PageButtons.Add(new PageButton
                {
                    Number = i,
                    CurrentPage = CurrentPage
                });
            }

            OnPropertyChanged(nameof(PageButtons));
        }

        public int ItemsPerPage { get; set; } = 15;
        public int TotalPages { get; set; }
        public ObservableCollection<PageButton> PageButtons { get; set; } = new();
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand GoToPageCommand { get; }

        private void GoToPage(int v)
        {
            if (v < 1 || v > TotalPages)
            {
                return;
            }
            CurrentPage = v;
            LoadPage();
        }

        private void UpdatePagination()
        {
            TotalPages = (int)Math.Ceiling(
                (double)NotificationList.Count / ItemsPerPage
            );

            UpdatePageButtons();
            LoadPage();

            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(PageButtons));
        }


        private void LoadPage()
        {
            if (CurrentPage < 1)
                CurrentPage = 1;

            PagedNotifications.Clear();

            var items = NotificationList
                .Skip((CurrentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            foreach (var item in items)
                PagedNotifications.Add(item);

            OnPropertyChanged(nameof(PagedNotifications));
        }


    }
}
