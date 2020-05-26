using StatefulPatternFunctions.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Web.Models.CertificationProfiles
{
    public class CreateModel
    {
        [Required]
        public string FirstName { get; set; }
   
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
