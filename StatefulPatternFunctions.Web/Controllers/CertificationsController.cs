using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatefulPatternFunctions.Core.Interfaces;
using StatefulPatternFunctions.Web.Models.Certifications;

namespace StatefulPatternFunctions.Web.Controllers
{
    public class CertificationsController : Controller
    {
        private readonly ILogger<CertificationsController> _logger;
        private readonly ICertificationProfilesProvider _certificationProfilesProvider;

        public CertificationsController(ICertificationProfilesProvider certificationProfilesProvider,
            ILogger<CertificationsController> logger)
        {
            if (certificationProfilesProvider == null)
                throw new ArgumentNullException(nameof(certificationProfilesProvider));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this._certificationProfilesProvider = certificationProfilesProvider;
            this._logger = logger;
        }

        public async Task<ActionResult> Details(Guid profileId, Guid certificationId)
        {
            var model = new DetailModel();
            var profile = await this._certificationProfilesProvider.GetCertificationProfileAsync(profileId, default);
            if (profile == null)
                return RedirectToAction(nameof(Index));
            var certification = profile.Certifications.FirstOrDefault(c => c.Id == certificationId);
            if (certification == null)
                return RedirectToAction(nameof(Index));

            model.CertificationId = certification.Id;
            model.CertificationName = certification.Name;
            model.CredentialId = certification.CredentialId;
            model.CredentialUrl = certification.CredentialUrl;
            model.ExpirationDate = certification.ExpirationDate;
            model.IssueDate = certification.IssueDate;
            model.IssuingOrganization = certification.IssuingOrganization;
            model.ProfileId = profile.Id;
            model.LastName = profile.LastName;
            model.FirstName = profile.FirstName;
            model.Email = profile.Email;

            return View(model);
        }

        // GET: CertificationsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CertificationsController/Create
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

        // GET: CertificationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CertificationsController/Edit/5
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

        // GET: CertificationsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CertificationsController/Delete/5
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
