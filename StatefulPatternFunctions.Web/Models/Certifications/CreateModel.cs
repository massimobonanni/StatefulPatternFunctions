using StatefulPatternFunctions.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Web.Models.Certifications
{
    public class CreateModel
    {
        public Guid ProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [Required]
        public string CertificationName { get; set; }
        [Required]
        public string IssuingOrganization { get; set; }

        public string CredentialUrl { get; set; }

        public string CredentialId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ExpirationDate { get; set; }
    }
}
