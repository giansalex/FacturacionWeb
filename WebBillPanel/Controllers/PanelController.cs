using System;
using System.IO;
using System.Web.Mvc;
using WebBusinessLayer;
using WebBusinessLayer.Security;
using WebDocs;

namespace WebBillPanel.Controllers
{
    public class PanelController : Controller
    {
        // GET: Panel
        public ActionResult Index()
        {
            if (Session["_idUser__"] == null) return RedirectToAction("", "Home");
            var idUser = Session["_idUser__"].ToString();
            var vbl = new VentaBl();
            //var cbl = new ClienteBl();
            //ViewBag.Cliente = cbl.GetClient(idUser);
            var items = vbl.GetListFromClient(idUser);
            return View(items);
        }

        public ActionResult OneDocument()
        {
            var idVenta = Session["_idVenta__"];
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
            Session["_idUser__"] = null;
            return RedirectToAction("LoginUser", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogoutDocument()
        {
            Session["_idVenta__"] = null;
            return RedirectToAction("LoginDoc", "Home");
        }

        [ActionName("gen-xml")]
        public FileResult GenerateXml(string id, string org)
        {
            switch (org)
            {
                case "uxsr":
                    if (Session["_idUser__"] == null) return null;
                    break;
                case "vb12d":
                    if (Session["_idVenta__"] == null) return null;
                    break;
                default:
                    return null;
            }

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
            return null;
        }

        [ActionName("gen-pdf")]
        public FileResult GeneratePdf(string id, string org)
        {
            switch (org)
            {
                case "uxsr":
                    if (Session["_idUser__"] == null) return null;
                    break;
                case "vb12d":
                    if (Session["_idVenta__"] == null) return null;
                    break;
                default:
                    return null;
            }
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
            return null;
        }

        public ActionResult SaveConfig()
        {
            var idVenta = Session["_idAdmin__"];
            if (idVenta == null) return RedirectToAction("Admin", "Home");
            ViewBag.IsSql = ManagerConfiguration.Bd == DataBases.SqlServer;
            ViewBag.Cadena = ManagerConfiguration.ConectionString;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveConfig(string tipo, string server, string user, string password, string database)
        {
            var idVenta = Session["_idAdmin__"];
            if (idVenta == null) return RedirectToAction("Admin", "Home");
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