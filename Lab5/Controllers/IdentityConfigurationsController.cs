using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lab5.Models;
using Lab5.Services;

namespace Lab5.Controllers
{
    public class IdentityConfigurationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IIdentityConfigurationsViewModelReader _reader = null;
        private IIdentityConfigurationsViewModelWriter _writer = null;

        public IdentityConfigurationsController()
        : this(new IdentityConfigurationsViewModelService(), new IdentityConfigurationsViewModelService()){
        }

        public IdentityConfigurationsController(IIdentityConfigurationsViewModelReader reader, IIdentityConfigurationsViewModelWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        // GET
        public async Task<ActionResult> Index()
        {
            var viewModel = await _reader.GetIdentityAndUsersConfigViewModelAsync();
            return View(viewModel);
        }

        // GET
        public async Task<ActionResult> EditIdentityConfigurations(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityConfigurationsViewModel model = await _reader.GetIdentityConfigViewModelAsync();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIdentityConfigurations([Bind(Include = "ID,MaxFailedAccessAttemptsBeforeLockout,DefaultAccountLockoutTimeSpan,RequiredLength, RequireNonLetterOrDigit, RequireDigit, RequireLowercase, RequireUppercase, CannotReusePassword")] IdentityConfigurationsViewModel identityConfiguration)
        {
            if (ModelState.IsValid)
            {
                _writer.EditIdentityConfigurations(identityConfiguration);
                return RedirectToAction("Index");
            }
            return View(identityConfiguration);
        }

        // GET
        public ActionResult EditUserConfigurations(string id)
        {
            var viewModel = _reader.GetUserViewModelAsync(id);
            return View(viewModel);
        }

        // POST
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserConfigurations([Bind(Include = "ID,UserName,LockoutEnabled")] ApplicationUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                _writer.EditUserConfigurations(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
