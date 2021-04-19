using CoreRDLCSubReport.Web.Models;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
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
            report.ReportPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\rptEmpleados.rdlc";
            report.SetParameters(parameters);

            //for sub report
            report.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessing);
            //

            var pdf = report.Render(renderFormat);
            return File(pdf, mimetype, "report." + extension);
        }

        void SubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            int id = int.Parse(e.Parameters["Id"].Values[0].ToString());
            var dt2 = new DataTable();
            dt2 = GetEmpleadosDetalle().Select("Id=" + id).CopyToDataTable();

            ReportDataSource ds = new ReportDataSource("dsEmpleadosDetalles", dt2);
            e.DataSources.Add(ds);
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

        public DataTable GetEmpleadosDetalle()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Detalles");
            DataRow row;
            for (int i = 100; i < 150; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    row = dt.NewRow();
                    row["Id"] = i;
                    row["Detalles"] = "Detalle " + j;
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

    }
}
