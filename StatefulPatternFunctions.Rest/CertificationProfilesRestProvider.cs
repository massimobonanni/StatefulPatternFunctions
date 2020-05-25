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
    public class CertificationProfilesRestProvider : RestClientBase, ICertificationProfilesProvider
    {
        public CertificationProfilesRestProvider(HttpClient httpClient,
            IConfiguration configuration) : base(httpClient, configuration)
        {
        }

        protected override HttpClient CreateHttpClient(string apiEndpoint)
        {
            HttpClient client;
            if (string.IsNullOrEmpty(apiEndpoint))
            {
                client = base.CreateHttpClient($"/api/profiles");
            }
            else
            {
                if (apiEndpoint.StartsWith("/"))
                    apiEndpoint = apiEndpoint.Remove(0, 1);
                client = base.CreateHttpClient($"/api/profiles/{apiEndpoint}");
            }
            return client;
        }

        public async Task<IEnumerable<CertificationProfilesGetModel>> GetCertificationProfilesAsync(CancellationToken token)
        {
            var client = CreateHttpClient(null);
            var response = await client.GetAsync("", token);
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
            var response = await client.GetAsync("", token);
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

        public async Task<bool> AddCertificationProfileAsync(
            CertificationProfileInitializeModel profile, CancellationToken token)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            using (var client = CreateHttpClient(""))
            {
                profile.Id = Guid.NewGuid();

                var profileJson = JsonSerializer.Serialize(profile,
                  new JsonSerializerOptions
                  {
                      PropertyNameCaseInsensitive = true,
                  });

                var postContent = new StringContent(profileJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("", postContent, token);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> UpdateCertificationProfileAsync(Guid id,
            CertificationProfileUpdateModel profile, CancellationToken token)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            using (var client = CreateHttpClient($"{id}"))
            {
                var profileJson = JsonSerializer.Serialize(profile,
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true,
                       });

                var putContent = new StringContent(profileJson, Encoding.UTF8, "application/json");

                var response = await client.PutAsync("", putContent, token);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteCertificationProfileAsync(Guid id, CancellationToken token)
        {
            using (var client = CreateHttpClient($"{id}"))
            {
                var response = await client.DeleteAsync("", token);

                return response.IsSuccessStatusCode;
            }
        }
    }
}
