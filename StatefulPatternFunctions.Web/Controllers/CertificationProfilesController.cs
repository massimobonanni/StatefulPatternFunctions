using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatefulPatternFunctions.Core.Interfaces;
using StatefulPatternFunctions.Core.Models;
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
            var profile = await this._certificationProfilesProvider.GetCertificationProfileAsync(id, default);
            if (profile == null)
                return RedirectToAction(nameof(Index));

            model.Profile = profile;

            return View(model);
        }

        public ActionResult Create()
        {
            var model = new CreateModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var profile = new CertificationProfileInitializeModel()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email
                    };
                    var result = await this._certificationProfilesProvider.AddCertificationProfileAsync(profile, default);
                    if (result)
                        return RedirectToAction(nameof(Index));

                    ModelState.AddModelError(string.Empty, "Error during inserting profile");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error during inserting profile");
                }
            }
            return View();
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var model = new EditModel();

            var profile = await this._certificationProfilesProvider.GetCertificationProfileAsync(id, default);
            if (profile == null)
                return RedirectToAction(nameof(Index));

            model.Id = profile.Id;
            model.Email = profile.Email;
            model.FirstName = profile.FirstName;
            model.LastName = profile.LastName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var profile = new CertificationProfileUpdateModel()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email
                    };
                    var result = await this._certificationProfilesProvider.UpdateCertificationProfileAsync(
                        model.Id, profile, default);
                    if (result)
                        return RedirectToAction(nameof(Index));

                    ModelState.AddModelError(string.Empty, "Error during updating profile");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error during updating profile");
                }
            }
            return View();
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            var model = new DeleteModel();
            var profile = await this._certificationProfilesProvider.GetCertificationProfileAsync(id, default);
            if (profile == null)
                return RedirectToAction(nameof(Index));

            model.Id = profile.Id;
            model.Email = profile.Email;
            model.FirstName = profile.FirstName;
            model.LastName = profile.LastName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteModel model)
        {
            try
            {
                var result = await this._certificationProfilesProvider.DeleteCertificationProfileAsync(model.Id, default);
                if (result)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Error during deleting profile");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error during deleting profile");
            }
            return View();
        }
    }
}
