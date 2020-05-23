using System;
using System.Collections.Generic;
using System.Text;

namespace StatefulPatternFunctions.CertificationManager
{
    internal static class Extensions
    {
        internal static Certification ToCertification(this CertificationUpsertModel model)
        {
            return new Certification()
            {
                CredentialId = model.CredentialId,
                CredentialUrl = model.CredentialUrl,
                ExpirationDate = model.ExpirationDate,
                Id = model.Id,
                IssueDate = model.IssueDate,
                IssuingOrganization = model.IssuingOrganization,
                Name = model.Name
            };
        }
    }
}
