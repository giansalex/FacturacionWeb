using System;
using System.IO;
using System.Security.Cryptography;
using System.Web.Mvc;
using WebBusinessLayer;
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
            return View(v);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session["_idUser__"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogoutDocument()
        {
            Session["_idVenta__"] = null;
            return RedirectToAction("Index", "Home");
        }

        public FileResult GenerateXml(string id)
        {
            if (Session["_idUser__"] == null) return null;
            var idVenta = Encryptor.Decode(id);
            var path = new XmlHelper().Generar(idVenta);
            if (System.IO.File.Exists(path))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = Path.GetFileName(path);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            return null;
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