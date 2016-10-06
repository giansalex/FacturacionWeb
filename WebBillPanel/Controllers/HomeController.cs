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
                    Session["_idAdmin__"] = id;
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
            Session["_idAdmin__"] = null;
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
                    Session["_idUser__"] = id;
                    return RedirectToAction("Index", "Panel");
                }
                ViewBag.NoValid = true;
            }
            return View();
        }

        //GET: Login Documento
        public ActionResult LoginDoc()
        {
            ViewBag.DocItems = GetDocs();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginDoc([Bind(Include = "TipoDocumento,Serie, Correlativo, FechaEmision, Total")] VentaCredencial factura)
        {
            if (ModelState.IsValid)
            {
                var idVenta = new VentaBl()
                    .GetIdVenta(factura.TipoDocumento, factura.Serie, factura.Correlativo, factura.FechaEmision, factura.Total);
                if (idVenta != null)
                {
                    Session["_idVenta__"] = idVenta;
                    return RedirectToAction("OneDocument", "Panel");
                }
                ViewBag.NoValid = true;
            }
            ViewBag.DocItems = GetDocs();
            return View();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Obtiene documentos electronicos disponibles.
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetDocs()
        {
            return new List<SelectListItem>
            {
                new SelectListItem(),
                new SelectListItem
                {
                    Text = @"FACTURA",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = @"BOLETA",
                    Value = "3"
                },
                new SelectListItem
                {
                    Text = @"NOTA DE CREDITO",
                    Value = "7"
                },
                new SelectListItem
                {
                    Text = @"NOTA DE DEBITO",
                    Value = "8"
                }
            };
        }

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