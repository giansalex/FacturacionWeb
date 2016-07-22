using System.Collections.Generic;
using System.Web.Mvc;
using WebBillPanel.Models;

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
                Session["_idUser__"] = usuario.UserName;
                return RedirectToAction("Index", "Panel");
            }
            return View();
        }

        //GET: Login Documento
        public ActionResult LoginDoc()
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem(),
                new SelectListItem
                {
                    Text = "FACTURA",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "BOLETA",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "NOTA DE CREDITO",
                    Value = "3"
                },
                new SelectListItem
                {
                    Text = "NOTA DE DEBITO",
                    Value = "4"
                }
            };
            ViewBag.DocItems = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginDoc([Bind(Include = "TipoDocumento,Serie, Documento, FechaEmision, Total")] VentaCredencial factura)
        {
            if (ModelState.IsValid)
            {
                return Redirect("http://www.google.com");
            }
            var list = new List<SelectListItem>
            {
                new SelectListItem(),
                new SelectListItem
                {
                    Text = "FACTURA",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "BOLETA",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "NOTA DE CREDITO",
                    Value = "3"
                },
                new SelectListItem
                {
                    Text = "NOTA DE DEBITO",
                    Value = "4"
                }
            };
            ViewBag.DocItems = list;
            return View();
        }
    }
}