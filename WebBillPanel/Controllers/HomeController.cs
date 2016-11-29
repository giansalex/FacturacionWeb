using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using WebBillPanel.Models;
using WebBusinessLayer;

namespace WebBillPanel.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        public const string IdUser = "_idUser__";
        public const string IdDoc = "_idVenta__";
        public const string IdAdmin = "_idAdmin__";
        #endregion

        #region Methods Controller
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin([Bind(Include = "UserName,Password")] ClienteCredencial admin)
        {
            if (ModelState.IsValid)
            {
                //var cbl = new ClienteBl();
                //var id = cbl.GetIdClient(admin.UserName, admin.Password);
                var id = "admin";
                if(admin.UserName == "10480048356" && admin.Password == "123456")
                //if (id != null)
                {
                    Session[IdAdmin] = id;
                    return RedirectToAction("SaveConfig", "Panel");
                }
                ViewBag.NoValid = true;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogoutAdmin()
        {
            Session[IdAdmin] = null;
            return RedirectToAction("Admin", "Home");
        }

        // GET: Login User
        public ActionResult LoginUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUser([Bind(Include = "UserName,Password")] ClienteCredencial usuario)
        {
            if (ModelState.IsValid)
            {
                var cbl = new ClienteBl();
                var pass = Encrypt(usuario.Password);
                var id = cbl.GetIdClient(usuario.UserName, pass);
                if (id != null)
                {
                    Session[IdUser] = id;
                    return RedirectToAction("Index", "Panel");
                }
                ViewBag.NoValid = true;
            }
            return View();
        }

        //GET: Login Documento
        public ActionResult LoginDoc()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginDoc([Bind(Include = "TipoDocumento,Serie, Correlativo, FechaEmision, Total")] VentaCredencial factura)
        {
            if (ModelState.IsValid)
            {
                var idVenta = new VentaBl()
                    .GetIdVenta((int)factura.TipoDocumento, factura.Serie, factura.Correlativo, factura.FechaEmision, factura.Total);
                if (idVenta != null)
                {
                    Session[IdDoc] = idVenta;
                    return RedirectToAction("OneDocument", "Panel");
                }
                ViewBag.NoValid = true;
            }
            return View();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Encrypts the specified p data.
        /// </summary>
        /// <param name="pData">The p data.</param>
        /// <returns>System.String.</returns>
        public static string Encrypt(string pData)
        {
            var parser = new UnicodeEncoding();
            byte[] encrypt = new MD5CryptoServiceProvider().ComputeHash(parser.GetBytes(pData));
            return Convert.ToBase64String(encrypt);
        }
        #endregion
    }
}