using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StatefulPatternFunctions.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.CertificationManager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CertificationProfileEntity
    {
        private readonly ILogger logger;

        public CertificationProfileEntity(ILogger logger)
        {
            this.logger = logger;
        }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("isInitialized")]
        public bool IsInitialized { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [JsonProperty("certifications")]
        public List<Certification> Certifications { get; set; } = new List<Certification>();

        public bool InitializeProfile(CertificationProfileInitializeModel profile)
        {
            if (IsInitialized || IsDeleted)
                return false;

            this.IsInitialized = true;
            this.LastName = profile.LastName;
            this.FirstName = profile.FirstName;
            this.Email = profile.Email;

            return true;
        }

        public bool DeleteProfile()
        {
            this.IsInitialized = false;
            this.LastName = null;
            this.FirstName = null;
            this.Email = null;
            this.Certifications.Clear();

            this.IsDeleted = true;

            return true;
        }

        public bool UpdateProfile(CertificationProfileInitializeModel profile)
        {
            if (!IsInitialized || IsDeleted)
                return false;
            this.LastName = profile.LastName;
            this.FirstName = profile.FirstName;
            this.Email = profile.Email;

            return true;
        }

        public bool UpsertCertification(CertificationUpsertModel certification)
        {
            if (!IsInitialized || IsDeleted)
                return false;

            var innerCertification = Certifications.FirstOrDefault(p => p.Id == certification.Id);
            if (innerCertification != null)
            {
                Certifications.Remove(innerCertification);
            }

            Certifications.Add(certification.ToCertification());
            return true;
        }

        public bool RemoveCertification(Guid certificationId)
        {
            if (!IsInitialized || IsDeleted)
                return false;

            var innerCertification = Certifications.FirstOrDefault(p => p.Id == certificationId);
            if (innerCertification != null)
            {
                throw new Exception("Certification not exist");
            }
            Certifications.Remove(innerCertification);
            return true;
        }

        public bool CleanCertifications()
        {
            if (!IsInitialized || IsDeleted)
                return false;

            Certifications.Clear();
            return true;
        }

        [FunctionName(nameof(CertificationProfileEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx, ILogger logger)
            => ctx.DispatchAsync<CertificationProfileEntity>(logger);
    }
}
