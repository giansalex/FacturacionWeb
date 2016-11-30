using System;
using System.IO;
using System.Web.Mvc;
using WebBusinessLayer;
using WebBusinessLayer.Security;
using System.Collections.Generic;
using WebDocs;
using WebDataModel.Entities;
using WebBillPanel.Models;

namespace WebBillPanel.Controllers
{
    public class PanelController : Controller
    {
        // GET: Panel
        public ActionResult Index()
        {
            if (Session[HomeController.IdUser] == null) return RedirectToAction("", "Home");
            var idUser = Session[HomeController.IdUser].ToString();
            //var vbl = new VentaBl();
            //var cbl = new ClienteBl();
            //ViewBag.Cliente = cbl.GetClient(idUser);
            //var items = vbl.GetListFromClient(idUser);
            var items = new List<ventaDto>
            {
                new ventaDto
                {
                    v_IdVenta = "N001-21312",
                    v_SerieDocumento = "F001",
                    v_CorrelativoDocumento ="000000012",
                    d_Total = 12.32M,
                    t_FechaRegistro = new DateTime(2016, 2,1)
                }
            };
            return View(items);
        }

        public ActionResult OneDocument()
        {
            var idVenta = Session[HomeController.IdDoc];
            if (idVenta == null) return RedirectToAction("Index", "Home");
            var vbl = new VentaBl();
            var v = vbl.GetVenta(idVenta.ToString());
            if (!vbl.LastResult.Success) return HttpNotFound("Error de Conexion");
            return View(v);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session[HomeController.IdUser] = null;
            return RedirectToAction("LoginUser", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogoutDocument()
        {
            Session[HomeController.IdDoc] = null;
            return RedirectToAction("LoginDoc", "Home");
        }

        [ActionName("gen-xml")]
        public ActionResult GenerateXml(string id)
        {
            if (IsNotValidSession()) return HttpNotFound();
            try
            {
                var idVenta = Encryptor.Decode(id);
                var path = new XmlHelper().Generar(idVenta);
                if (System.IO.File.Exists(path))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                    System.IO.File.Delete(path);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(path));
                }
            }
            catch (Exception er)
            {
                ExceptionUtility.LogException(er, "GenerateXMl");
            }
            return HttpNotFound();
        }

        [ActionName("gen-pdf")]
        public ActionResult GeneratePdf(string id)
        {
            if (IsNotValidSession()) return HttpNotFound();
            try
            {
                var idVenta = Encryptor.Decode(id);
                var content = new RepFacturacionElectronica().GenerarPdf(idVenta);
                if (content != null)
                    return File(content.Item2, System.Net.Mime.MediaTypeNames.Application.Pdf, content.Item1);
            }
            catch (Exception er)
            {
                ExceptionUtility.LogException(er, "GeneratePDF");
            }
            return HttpNotFound();
        }

        public ActionResult SaveConfig()
        {
            if (Session[HomeController.IdAdmin] == null) return RedirectToAction("Admin", "Home");
            ViewBag.IsSql = ManagerConfiguration.Bd == DataBases.SqlServer;
            ViewBag.Cadena = ManagerConfiguration.ConectionString;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveConfig(string tipo, string server, string user, string password, string database)
        {
            if (Session[HomeController.IdAdmin] == null) return RedirectToAction("Admin", "Home");
            DataBases db;
            if (Enum.TryParse(tipo, out db))
            {
                ManagerConfiguration.Save(db, server, user, password, database);
                ViewBag.IsValid = true;
                ViewBag.Mensaje = "Los cambios se guardaron correctamente.";
            }
            else
            {
                ViewBag.IsValid = false;
                ViewBag.Mensaje = "El tipo de Base de Datos no es aceptado";
            }

            ViewBag.IsSql = db == DataBases.SqlServer;
            return View();
        }

        [HttpPost]
        public ActionResult Search([Bind(Include = "TipoDocumento,Serie,Correlativo,FechaInicial,FechaFinal")] FilterDoc filter)
        {
            if (Session[HomeController.IdUser] == null) return RedirectToAction("", "Home");
            var idUser = Session[HomeController.IdUser].ToString();

            var filters = new Dictionary<string, string>();
            if(filter.TipoDocumento > 0)
                filters.Add("i_IdTipoDocumento", filter.TipoDocumento.ToString());

            if (!string.IsNullOrEmpty(filter.Serie))
                filters.Add("v_SerieDocumento", $"'{filter.Serie}'");

            if (!string.IsNullOrEmpty(filter.Correlativo))
                filters.Add("v_CorrelativoDocumento", $"'{filter.Correlativo}'");

            var bl = new VentaBl();
            var items = bl.SearchVentas(idUser, filter.FechaInicial, filter.FechaFinal, filters);
            if (!bl.LastResult.Success)
            {
                return HttpNotFound(bl.LastResult.ErrorMessage);
            }
            return View(items);
        }
        private bool IsNotValidSession()
        {
            return Session[HomeController.IdDoc] == null && Session[HomeController.IdUser] == null;
        }
    }

    public static class Encryptor
    {
        public static string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
    }

    public sealed class ExceptionUtility
    {
        // All methods are static, so this can be private 
        private ExceptionUtility()
        {
        }

        // Log an Exception 
        public static void LogException(Exception exc, string source)
        {
            // Include enterprise logic for logging exceptions 
            // Get the absolute path to the log file 

            // Open the log file for append and write the log
            using (StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/log.txt"), true))
            {
                sw.WriteLine("********** {0} **********", DateTime.Now);
                if (exc.InnerException != null)
                {
                    sw.Write("Inner Exception Type: ");
                    sw.WriteLine(exc.InnerException.GetType().ToString());
                    sw.Write("Inner Exception: ");
                    sw.WriteLine(exc.InnerException.Message);
                    sw.Write("Inner Source: ");
                    sw.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.StackTrace);
                    }
                }
                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);
                sw.WriteLine("Source: " + source);
                sw.WriteLine("Stack Trace: ");
                if (exc.StackTrace != null)
                {
                    sw.WriteLine(exc.StackTrace);
                    sw.WriteLine();
                }
                sw.Close();
            }
        }

        // Notify System Operators about an exception 
        public static void NotifySystemOps(Exception exc)
        {
            // Include code for notifying IT system operators
        }
    }
}