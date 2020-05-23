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
                ExpirationDate = model.ExpirationDate?.Date,
                Id = model.Id,
                IssueDate = model.IssueDate.HasValue ? model.IssueDate.Value : DateTime.Today,
                IssuingOrganization = model.IssuingOrganization,
                Name = model.Name
            };
        }
    }
}
