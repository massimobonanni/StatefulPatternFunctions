using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Web.Models.Generator
{
    public class CreateModel
    {
        [Required]
        [Range(1,50)]
        public int NumberOfProfiles { get; set; }
    }
}
