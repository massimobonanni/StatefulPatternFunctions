using System;
using System.Collections.Generic;
using System.Text;

namespace StatefulPatternFunctions.Core.Models
{
    public class CertificationProfileInitializeModel
    {
        public Guid Id { get; set; }

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

    public class CertificationProfileGetModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<CertificationGetModel> Certifications { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class CertificationProfilesGetModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
