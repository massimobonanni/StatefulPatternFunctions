using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatefulPatternFunctions.Core.Interfaces;
using StatefulPatternFunctions.Web.Models.CertificationProfiles;

namespace StatefulPatternFunctions.Web.Controllers
{
    public class CertificationProfilesController : Controller
    {
        private readonly ILogger<CertificationProfilesController> _logger;
        private readonly ICertificationProfilesProvider _certificationProfilesProvider;

        public CertificationProfilesController(ICertificationProfilesProvider certificationProfilesProvider,
            ILogger<CertificationProfilesController> logger)
        {
            if (certificationProfilesProvider == null)
                throw new ArgumentNullException(nameof(certificationProfilesProvider));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this._certificationProfilesProvider = certificationProfilesProvider;
            this._logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var model = new IndexModel();
            var profiles = await this._certificationProfilesProvider.GetCertificationProfilesAsync(default);

            model.Profiles = profiles.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
            return View(model);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var model = new DetailModel();
            var profile = await this._certificationProfilesProvider.GetCertificationProfileAsync(id,default);
            if (profile == null)
                return RedirectToAction(nameof(Index));

            model.Profile  = profile;

            return View(model);
        }

        // GET: CertificationProfilesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CertificationProfilesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CertificationProfilesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CertificationProfilesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CertificationProfilesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CertificationProfilesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
