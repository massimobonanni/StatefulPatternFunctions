using System;
using System.Collections.Generic;
using System.Text;

namespace StatefulPatternFunctions.Core.Models
{
    public class CertificationUpsertModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string IssuingOrganization { get; set; }

        public string CredentialUrl { get; set; }

        public string CredentialId { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }

    public class CertificationGetModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IssuingOrganization { get; set; }

        public string CredentialUrl { get; set; }

        public string CredentialId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
