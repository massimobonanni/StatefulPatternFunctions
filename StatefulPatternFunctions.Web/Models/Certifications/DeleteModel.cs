using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Web.Models.Certifications
{
    public class DeleteModel
    {

        public Guid ProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Guid CertificationId { get; set; }

        public string CertificationName { get; set; }

        public string IssuingOrganization { get; set; }

        public string CredentialUrl { get; set; }

        public string CredentialId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
