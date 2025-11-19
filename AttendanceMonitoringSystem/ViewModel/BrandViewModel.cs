//BrandViewModel class added by the syncfusion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceMonitoringSystem.Model;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class BrandViewModel
    {
        
        public ObservableCollection<BrandModel> SalesData { get; }

        public ObservableCollection<BrandModel> Data { get; }

        public BrandViewModel()
        {
            this.SalesData = new ObservableCollection<BrandModel>();
            SalesData.Add(new BrandModel() { Name = "Product A", SalesRate = 25 });
            SalesData.Add(new BrandModel() { Name = "Product B", SalesRate = 17 });
            SalesData.Add(new BrandModel() { Name = "Product C", SalesRate = 30 });
            SalesData.Add(new BrandModel() { Name = "Product D", SalesRate = 18 });
            SalesData.Add(new BrandModel() { Name = "Product E", SalesRate = 10 });
            SalesData.Add(new BrandModel() { Name = "Product F", SalesRate = 21 });


            this.Data = new ObservableCollection<BrandModel>();
            Data.Add(new BrandModel() { Name = "Product A", Imports = 2.2, Exports = 1.2 });
            Data.Add(new BrandModel() { Name = "Product B", Imports = 2.4, Exports = 1.3 });
            Data.Add(new BrandModel() { Name = "Product C", Imports = 3, Exports = 1.5 });
            Data.Add(new BrandModel() { Name = "Product D", Imports = 3.1, Exports = 2.2 });


        
        }
        

    }
}
