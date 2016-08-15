using System.Security.Cryptography;
using System.Web.Mvc;
using WebBusinessLayer;

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
    }
}