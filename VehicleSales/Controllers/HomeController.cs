using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleSales.Models;
using VehicleSales.ViewModelBuilder.Home;

namespace VehicleSales.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(VehicleSalesModel viewModel)
        {
            var builder = new IndexBuilder(viewModel.FileUpload);

            builder.Build(ModelState, viewModel);

            return View(viewModel);
        }
    }
}