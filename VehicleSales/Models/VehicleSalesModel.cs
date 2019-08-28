using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleSales.Models
{
    public class VehicleSalesModel
    {
        public HttpPostedFileBase FileUpload { get; set; }
        public List<SalesModel> SalesList { get; set; }
        public string MostSoldVehicle { get; set; }
    }
    public class SalesModel
    {
        public int DealNumber { get; set; }
        public string CustomerName { get; set; }
        public string DealershipName { get; set; }
        public string Vehicle { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}