using Microsoft.Extensions.Configuration;
using StatefulPatternFunctions.Core.Interfaces;
using StatefulPatternFunctions.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Rest
{
    public class CertificatioProfilesRestProvider : RestClientBase, ICertificationProfilesProvider
    {
        public CertificatioProfilesRestProvider(HttpClient httpClient,
            IConfiguration configuration) : base(httpClient, configuration)
        {
        }

        protected override HttpClient CreateHttpClient(string apiEndpoint)
        {
            if (string.IsNullOrEmpty(apiEndpoint))
                return base.CreateHttpClient($"/api/profiles");

            if (apiEndpoint.StartsWith("/"))
                apiEndpoint = apiEndpoint.Remove(0, 1);
            return base.CreateHttpClient($"/api/profiles/{apiEndpoint}");
        }

        public async Task<IEnumerable<CertificationProfilesGetModel>> GetCertificationProfilesAsync(CancellationToken token)
        {
            var client = CreateHttpClient(null);
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var profiles = JsonSerializer.Deserialize<List<CertificationProfilesGetModel>>(content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return profiles;
            }
            return null;
        }

        public async Task<CertificationProfileGetModel> GetCertificationProfileAsync(Guid profileId, CancellationToken token)
        {
            var client = CreateHttpClient($"{profileId}");
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var profile = JsonSerializer.Deserialize<CertificationProfileGetModel>(content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return profile;
            }
            return null;

        }
    }
}
