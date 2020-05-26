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

        Task<CertificationProfileGetModel> GetCertificationProfileAsync(Guid profileId, CancellationToken token);

        Task<bool> AddCertificationProfileAsync(CertificationProfileInitializeModel profile, CancellationToken token);

        Task<bool> UpdateCertificationProfileAsync(Guid id, CertificationProfileUpdateModel profile, CancellationToken token);

        Task<bool> DeleteCertificationProfileAsync(Guid id, CancellationToken token);

        Task<bool> AddCertificationAsync(Guid profileId, CertificationUpsertModel certification, CancellationToken token);

        Task<bool> UpdateCertificationAsync(Guid profileId, CertificationUpsertModel certification, CancellationToken token);

        Task<bool> DeleteCertificationAsync(Guid profileId, Guid certificationId, CancellationToken token);

    }
}
