using System.Web.Mvc;

namespace Lab5.Controllers
{
    public class ShapeController : Controller
    {

        // GET: Circle
        [Authorize(Roles="Préposé au cercle, Administrateur")]
        public ActionResult Circle()
        {
            return View();
        }

        // GET: Square
        [Authorize(Roles = "Préposé au carré, Administrateur")]
        public ActionResult Square()
        {
            return View();
        }
    }
}