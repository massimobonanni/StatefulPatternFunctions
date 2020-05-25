using StatefulPatternFunctions.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Web.Models.CertificationProfiles
{
    public class IndexModel
    {
        public IEnumerable<CertificationProfilesGetModel> Profiles { get; set; }
    }
}
