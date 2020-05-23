using System;
using System.Collections.Generic;
using System.Text;

namespace StatefulPatternFunctions.CertificationManager
{
    public class CertificationProfileInitializeModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }

    public class CertificationProfileUpdateModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
