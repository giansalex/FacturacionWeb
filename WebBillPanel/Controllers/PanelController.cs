using System;
using System.IO;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
            var cbl = new ClienteBl();
            ViewBag.Cliente = cbl.GetClient(idUser);
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
            ViewBag.Config = new ConfiguracionFacturacionBl().GetLite();
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
        public FileResult GenerateXml(string id, string pred)
        {
            if(pred == "uxsr" && Session["_idUser__"] == null)
                return null;
            if (pred == "vb12d" && Session["_idVenta__"] == null)
                return null;
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
            catch
            {
                // ignored
            }
            return null;
        }

        [ActionName("gen-pdf")]
        public FileResult GeneratePdf(string id,string org)
        {
            if (org == "uxsr" && Session["_idUser__"] == null)
                return null;
            if (org == "vb12d" && Session["_idVenta__"] == null)
                return null;
            try
            {
                var idVenta = Encryptor.Decode(id);
                var path = new RepFacturacionElectronica().GenerarPdf(idVenta);
                if (!string.IsNullOrEmpty(path))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                    System.IO.File.Delete(path);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, Path.GetFileName(path));
                }
            }
            catch
            {
                // ignored
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
        public ActionResult SaveConfig(string tipo,string server,string user, string password, string database)
        {
            var idVenta = Session["_idAdmin__"];
            if (idVenta == null) return RedirectToAction("Admin", "Home");
            DataBases db;
            if (Enum.TryParse(tipo, out db))
            {
                ManagerConfiguration.Save(db, server,user, password, database);
                ViewBag.IsValid = true;
                ViewBag.Mensaje = "Los cambios se guardaron correctamente.";
            }
            else
            {
                ViewBag.IsValid = false;
                ViewBag.Mensaje = "El tipo de Base de Datos no es aceptado";
            }
            ViewBag.IsSql = db == DataBases.SqlServer;;
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
}