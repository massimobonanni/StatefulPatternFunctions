using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatefulPatternFunctions.Core.Interfaces;
using StatefulPatternFunctions.Web.Models.Generator;

namespace StatefulPatternFunctions.Web.Controllers
{
    public class GeneratorController : Controller
    {
        private readonly ILogger<GeneratorController> _logger;
        private readonly IProfilesGenerator _profileGenerator;

        public GeneratorController(IProfilesGenerator profileGenerator,
            ILogger<GeneratorController> logger)
        {
            if (profileGenerator == null)
                throw new ArgumentNullException(nameof(profileGenerator));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this._profileGenerator = profileGenerator;
            this._logger = logger;
        }

        public ActionResult Create()
        {
            var model = new CreateModel();
            return View(model);
        }

        // POST: GeneratorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this._profileGenerator.GenerateProfilesAsync(model.NumberOfProfiles, default);

                    return RedirectToAction("Home", "Index");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error during generating profiles");
                }
            }
            return View();
        }
    }
}
