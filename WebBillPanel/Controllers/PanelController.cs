using System.Web.Mvc;

namespace WebBillPanel.Controllers
{
    public class PanelController : Controller
    {
        // GET: Panel
        public ActionResult Index()
        {
            if (Session["_idUser__"] == null) return RedirectToAction("", "Home");

            return View();
        }

        public ActionResult OneDocument()
        {
            return View();
        }
    }
}