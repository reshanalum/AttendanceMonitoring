using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AttendanceMonitoringSystem.Command
{
    public class PageEqualsConverter : IMultiValueConverter
    {
        //public object Convert(object[] values, Type t, object p, CultureInfo c)
        //{
        //    if (values[0] == null || values[1] == null)
        //        return Application.Current.FindResource("PageButtonStyle");

        //    int currentPage = (int)values[0];
        //    int pageNumber = (int)values[1];

        //    if (currentPage == pageNumber)
        //        return Application.Current.FindResource("ActivePageButtonStyle");
        //    else
        //        return Application.Current.FindResource("PageButtonStyle");
        //}


        //public object[] ConvertBack(object v, Type[] t, object p, CultureInfo c)
        //    => throw new NotImplementedException(); 

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;

            if (values[0] == null || values[1] == null)
                return false;

            int currentPage = (int)values[0];
            int pageNumber = (int)values[1];

            return currentPage == pageNumber;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
