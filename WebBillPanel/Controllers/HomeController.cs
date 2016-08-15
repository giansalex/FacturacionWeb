using System.Collections.Generic;
using System.Web.Mvc;
using WebBillPanel.Models;
using WebBusinessLayer;

namespace WebBillPanel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
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
                var id = cbl.GetIdClient(usuario.UserName, usuario.Password);
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
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = @"NOTA DE CREDITO",
                    Value = "3"
                },
                new SelectListItem
                {
                    Text = @"NOTA DE DEBITO",
                    Value = "4"
                }
            };
        }
    }
}