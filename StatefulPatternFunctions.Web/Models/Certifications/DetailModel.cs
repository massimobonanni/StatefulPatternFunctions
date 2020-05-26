using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Web.Models.Certifications
{
    public class DetailModel
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

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime IssueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ExpirationDate { get; set; }
    }
}
