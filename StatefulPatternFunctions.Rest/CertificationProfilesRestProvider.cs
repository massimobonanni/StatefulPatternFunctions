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
                
        protected override Uri CreateAPIUri(string apiEndpoint)
        {
            Uri uri;
            if (string.IsNullOrEmpty(apiEndpoint))
            {
                uri = base.CreateAPIUri($"/api/profiles");
            }
            else
            {
                if (apiEndpoint.StartsWith("/"))
                    apiEndpoint = apiEndpoint.Remove(0, 1);
                uri = base.CreateAPIUri($"/api/profiles/{apiEndpoint}");
            }

            return uri;
        }

        public async Task<IEnumerable<CertificationProfilesGetModel>> GetCertificationProfilesAsync(CancellationToken token)
        {
            var uri = this.CreateAPIUri("");
            var response = await this._httpClient.GetAsync(uri, token);
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
            var uri = this.CreateAPIUri($"{profileId}");

            var response = await this._httpClient.GetAsync(uri, token);
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

            var uri = this.CreateAPIUri("");

            profile.Id = Guid.NewGuid();

            var profileJson = JsonSerializer.Serialize(profile,
              new JsonSerializerOptions
              {
                  PropertyNameCaseInsensitive = true,
              });

            var postContent = new StringContent(profileJson, Encoding.UTF8, "application/json");

            var response = await this._httpClient.PostAsync(uri, postContent, token);

            return response.IsSuccessStatusCode;

        }

        public async Task<bool> UpdateCertificationProfileAsync(Guid id,
            CertificationProfileUpdateModel profile, CancellationToken token)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            var uri = this.CreateAPIUri($"{id}");

            var profileJson = JsonSerializer.Serialize(profile,
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true,
                   });

            var putContent = new StringContent(profileJson, Encoding.UTF8, "application/json");

            var response = await this._httpClient.PutAsync(uri, putContent, token);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCertificationProfileAsync(Guid id, CancellationToken token)
        {
            var uri = this.CreateAPIUri($"{id}");
            var response = await this._httpClient.DeleteAsync(uri, token);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddCertificationAsync(Guid profileId, CertificationUpsertModel certification, CancellationToken token)
        {
            if (certification == null)
                throw new ArgumentNullException(nameof(certification));

            var uri = this.CreateAPIUri($"{profileId}/certifications");

            certification.Id = Guid.NewGuid();

            var profileJson = JsonSerializer.Serialize(certification,
                  new JsonSerializerOptions
                  {
                      PropertyNameCaseInsensitive = true,
                  });

            var postContent = new StringContent(profileJson, Encoding.UTF8, "application/json");

            var response = await this._httpClient.PostAsync(uri, postContent, token);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCertificationAsync(Guid profileId, CertificationUpsertModel certification, CancellationToken token)
        {
            if (certification == null)
                throw new ArgumentNullException(nameof(certification));

            var uri = this.CreateAPIUri($"{profileId}/certifications/{certification.Id}");

            var profileJson = JsonSerializer.Serialize(certification,
                  new JsonSerializerOptions
                  {
                      PropertyNameCaseInsensitive = true,
                  });

            var putContent = new StringContent(profileJson, Encoding.UTF8, "application/json");

            var response = await this._httpClient.PutAsync(uri, putContent, token);

            return response.IsSuccessStatusCode;

        }
    }
}
