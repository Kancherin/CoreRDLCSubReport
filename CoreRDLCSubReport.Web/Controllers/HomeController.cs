using CoreRDLCSubReport.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoreRDLCSubReport.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PrintReport()
        {
            string renderFormat = "PDF";
            string extension = "pdf";
            string mimetype = "application/pdf";
            using var report = new LocalReport();

            var dt = new DataTable();
            dt = GetEmpleados();

            report.DataSources.Add(new ReportDataSource("dsEmpleados", dt));
            var parameters = new[] { new ReportParameter("param1", "RDLC Sub-Report Ejemplo") };

            return View();
        }


        public DataTable GetEmpleados()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Departamento");
            dt.Columns.Add("Cargo");

            DataRow row;
            for (int i = 100; i < 150; i++)
            {
                row = dt.NewRow();
                row["Id"] = i;
                row["Nombre"] = "Empleado " + i;
                row["Departamento"] = "Sistemas de Información";
                row["Cargo"] = "Desarrollador";
                dt.Rows.Add(row);
            }
            return dt;
        }

        
    }
}
