using StatefulPatternFunctions.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Core.Interfaces
{
    public interface ICertificationProfilesProvider
    {
        Task<IEnumerable<CertificationProfilesGetModel>> GetCertificationProfilesAsync(CancellationToken token);


    }
}
