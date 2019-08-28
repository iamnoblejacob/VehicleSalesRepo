using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VehicleSales.Helpers
{
    public static class Helpers
    {
        public static string hlpr_CurrencyCAD(this HtmlHelper helper, object value)
        {
            return String.Format("CAD${0:n2}", value ?? 0);
        }

        public static String hlpr_DateToYYYYMMDD(this HtmlHelper helper, DateTime? _date)
        {
            if (_date == null || _date <= DateTime.Parse("1899-12-31"))
                return "";
            return _date.Value.ToString("yyyy/MM/dd");
        }

    }
}