using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatefulPatternFunctions.Core.Interfaces;
using StatefulPatternFunctions.Core.Models;
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
                return RedirectToAction("Index","CertificationProfiles");
            var certification = profile.Certifications.FirstOrDefault(c => c.Id == certificationId);
            if (certification == null)
                return RedirectToAction("Details", "CertificationProfiles", new { id = profileId });

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

        public async Task<ActionResult> Create(Guid profileId)
        {
            var model = new CreateModel();

            var profile = await this._certificationProfilesProvider.GetCertificationProfileAsync(profileId, default);
            if (profile == null)
                return RedirectToAction("Details", "CertificationProfiles", new { id = profileId });

            model.ProfileId = profile.Id;
            model.LastName = profile.LastName;
            model.FirstName = profile.FirstName;
            model.Email = profile.Email;

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
                    var certification = new CertificationUpsertModel()
                    {
                        CredentialId = model.CredentialId,
                        CredentialUrl = model.CredentialUrl,
                        ExpirationDate = model.ExpirationDate,
                        Id = Guid.NewGuid(),
                        IssueDate = model.IssueDate,
                        IssuingOrganization = model.IssuingOrganization,
                        Name = model.CertificationName
                    };

                    var result = await this._certificationProfilesProvider.AddCertificationAsync(model.ProfileId,
                        certification, default);
                    if (result)
                        return RedirectToAction("Details", "CertificationProfiles", new { id = model.ProfileId });

                    ModelState.AddModelError(string.Empty, "Error during inserting certification");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error during inserting certification");
                }
            }
            return View();
        }

        public async Task<ActionResult> Edit(Guid profileId, Guid certificationId)
        {
            var model = new EditModel();

            var profile = await this._certificationProfilesProvider.GetCertificationProfileAsync(profileId, default);
            if (profile == null)
                return RedirectToAction("Index", "CertificationProfiles");
            var certification = profile.Certifications.FirstOrDefault(c => c.Id == certificationId);
            if (certification == null)
                return RedirectToAction("Details", "CertificationProfiles", new { id = profileId });

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

        // POST: CertificationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var certification = new CertificationUpsertModel()
                    {
                        CredentialId = model.CredentialId,
                        CredentialUrl = model.CredentialUrl,
                        ExpirationDate = model.ExpirationDate,
                        Id = model.CertificationId,
                        IssueDate = model.IssueDate,
                        IssuingOrganization = model.IssuingOrganization,
                        Name = model.CertificationName
                    };

                    var result = await this._certificationProfilesProvider.UpdateCertificationAsync(model.ProfileId,
                        certification, default);

                    if (result)
                        return RedirectToAction("Details", "CertificationProfiles", new { id = model.ProfileId });

                    ModelState.AddModelError(string.Empty, "Error during updating certification");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error during updating certification");
                }
            }
            return View();
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
