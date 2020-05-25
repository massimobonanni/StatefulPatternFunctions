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

        [JsonProperty("certifications")]
        public List<Certification> Certifications { get; set; } = new List<Certification>();

        public bool InitializeProfile(CertificationProfileInitializeModel profile)
        {
            if (IsInitialized)
                return false;

            this.IsInitialized = true;
            this.LastName = profile.LastName;
            this.FirstName = profile.FirstName;
            this.Email = profile.Email;

            return true;
        }

        public bool UpdateProfile(CertificationProfileInitializeModel profile)
        {
            if (!IsInitialized)
                return false;
            this.LastName = profile.LastName;
            this.FirstName = profile.FirstName;
            this.Email = profile.Email;

            return true;
        }

        public bool UpsertCertification(CertificationUpsertModel certification)
        {
            if (!IsInitialized)
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
            if (!IsInitialized)
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
            Certifications.Clear();
            return true;
        }

        [FunctionName(nameof(CertificationProfileEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx, ILogger logger)
            => ctx.DispatchAsync<CertificationProfileEntity>(logger);
    }
}
