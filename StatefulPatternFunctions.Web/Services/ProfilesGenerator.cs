using Microsoft.Extensions.Logging;
using StatefulPatternFunctions.Core.Interfaces;
using StatefulPatternFunctions.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Web.Services
{
    public class ProfilesGenerator : IProfilesGenerator
    {
        private readonly ICertificationProfilesProvider _profilesProvider;
        private readonly ILogger<ProfilesGenerator> _logger;


        public ProfilesGenerator(ICertificationProfilesProvider profilesProvider,
            ILogger<ProfilesGenerator> logger)
        {
            if (profilesProvider == null)
                throw new ArgumentNullException(nameof(profilesProvider));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this._profilesProvider = profilesProvider;
            this._logger = logger;
        }

        public async Task GenerateProfilesAsync(int numberOfProfiles, CancellationToken token)
        {
            for (int i = 0; i < numberOfProfiles; i++)
            {
                var profile = new CertificationProfileInitializeModel();
                profile.Id = Guid.NewGuid();
                profile.FirstName = GenerateFirstName();
                profile.LastName = GenerateLastName();
                profile.Email = GenerateEmail(profile.LastName,profile.FirstName);

                if (await this._profilesProvider.AddCertificationProfileAsync(profile, token))
                {
                    var certifications = GenerateCertifications();
                    foreach (var certification in certifications)
                    {
                        await this._profilesProvider.AddCertificationAsync(profile.Id, certification, token);
                    }
                }
            }
        }

        private static Random rnd = new Random(DateTime.Now.Millisecond);


        private IEnumerable<CertificationUpsertModel> GenerateCertifications()
        {
            var selectedCertifications = new List<CertificationUpsertModel>();
            var numCertification = rnd.Next(0, certifications.Count);
            while (selectedCertifications.Count() <= numCertification)
            {
                var index = rnd.Next(0, certifications.Count - 1);
                var selectedCertification = certifications.ElementAt(index);
                if (!selectedCertifications.Any(c => c.Name == selectedCertification.Name))
                {
                    var certification = new CertificationUpsertModel();
                    certification.CredentialId = selectedCertification.CredentialId;
                    certification.CredentialUrl = selectedCertification.CredentialUrl;
                    certification.IssuingOrganization = selectedCertification.IssuingOrganization;
                    certification.Name = selectedCertification.Name;
                    certification.IssueDate = GenerateDate(DateTime.Now.AddDays(-365), DateTime.Now).Date;
                    if (selectedCertification.ExpirationDate.HasValue)
                    {
                        certification.ExpirationDate = certification.IssueDate.Value.AddYears(selectedCertification.ExpirationDate.Value.Year);
                    }
                    selectedCertifications.Add(certification);
                }
            }
            return selectedCertifications;
        }

        private DateTime GenerateDate(DateTime from, DateTime to)
        {
            var diff = to.Subtract(from);
            var days = rnd.Next(0, (int)diff.TotalDays);
            return to.AddDays(days);
        }

        private string GenerateFirstName()
        {
            var index = rnd.Next(0, firstNames.Count - 1);
            return firstNames.ElementAt(index);
        }

        private string GenerateLastName()
        {
            var index = rnd.Next(0, lastNames.Count - 1);
            return lastNames.ElementAt(index);
        }

        private string GenerateEmail(string lastName, string firstName)
        {
            var index = rnd.Next(0, emailDomains.Count - 1);
            return $"{firstName}.{lastName}@{emailDomains.ElementAt(index)}";
        }

        private static List<string> firstNames = new List<string>()
        {
            "Mario","Giovanni","Filippo","Andrea","Carla","Maria","Carmela","Giovanna","Filippa","Massimo","Marcello",
            "Marcella","Camillo","Camilla","Luca","Giancarlo","Gianmarco","Luciano","Luciana","Giuseppe"
        };

        private static List<string> lastNames = new List<string>()
        {
            "Rossi","Bianchi","Verdi","Russo","Ferrari","Esposito","Romano","Colombo","Ricci","Marino","Greco","Bruno","Gallo","Conti","De Luca","Mancini","Costa",
            "Giordano","Rizzo","Lombardi"
        };

        private static List<string> emailDomains = new List<string>()
        {
            "outlook.com","outlook.it","hotmail.com","hotmail.it","google.com","yahoo.com","email.it"
        };

        private static List<CertificationUpsertModel> certifications = new List<CertificationUpsertModel>()
        {
            new CertificationUpsertModel()
            {
                 Name="AZ-900: Microsoft Azure Fundamentals",
                 IssuingOrganization="Microsoft"
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-500: Microsoft Azure Security Technologies",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-400: Microsoft Azure DevOps Solutions",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-301: Microsoft Azure Architect Design",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-300: Microsoft Azure Architect Technologies",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-220: Microsoft Azure IoT Developer",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-204: Developing Solutions for Microsoft Azure",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-203: Developing Solutions for Microsoft Azure",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-120: Planning and Administering Microsoft Azure for SAP Workloads",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-104: Microsoft Azure Administrator",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AZ-103: Microsoft Azure Administrator",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            },
            new CertificationUpsertModel()
            {
                 Name="AI-100: Designing and Implementing an Azure AI Solution",
                 IssuingOrganization="Microsoft",
                 ExpirationDate=new DateTime(2,1,1)
            }
        };
    }
}
