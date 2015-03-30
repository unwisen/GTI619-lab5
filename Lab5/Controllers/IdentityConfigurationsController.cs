using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lab5.Models;
using Lab5.Services;

namespace Lab5.Controllers
{
    public class IdentityConfigurationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IIdentityConfigurationsViewModelReader reader = null;

        public IdentityConfigurationsController()
        : this(new IdentityConfigurationsViewModelService()){
        }

        public IdentityConfigurationsController(IIdentityConfigurationsViewModelReader reader)
        {
            this.reader = reader;
        }

        // GET: IdentityConfigurations
        public ActionResult Index()
        {
            var viewModel = reader.GetIdentityConfigurationsViewModel();
            return View(viewModel);
        }

        // GET: IdentityConfigurations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityConfiguration identityConfiguration = db.IdentityConfigurations.Find(id);
            if (identityConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(identityConfiguration);
        }

        // GET: IdentityConfigurations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IdentityConfigurations/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaxFailedAccessAttemptsBeforeLockout,DefaultAccountLockoutTimeSpan,SetLockoutEndDate")] IdentityConfiguration identityConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.IdentityConfigurations.Add(identityConfiguration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(identityConfiguration);
        }

        // GET: IdentityConfigurations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityConfiguration identityConfiguration = db.IdentityConfigurations.Find(id);
            if (identityConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(identityConfiguration);
        }

        // POST: IdentityConfigurations/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaxFailedAccessAttemptsBeforeLockout,DefaultAccountLockoutTimeSpan,SetLockoutEndDate")] IdentityConfiguration identityConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(identityConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(identityConfiguration);
        }

        // GET: IdentityConfigurations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityConfiguration identityConfiguration = db.IdentityConfigurations.Find(id);
            if (identityConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(identityConfiguration);
        }

        // POST: IdentityConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IdentityConfiguration identityConfiguration = db.IdentityConfigurations.Find(id);
            db.IdentityConfigurations.Remove(identityConfiguration);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult LockUser(int id)
        {
            var user = db.Users.ToList();
            if (user[id].LockoutEnabled)
            {
                user[id].LockoutEnabled = false;
            }
            else
            {
                user[id].LockoutEnabled = true;
            }
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
