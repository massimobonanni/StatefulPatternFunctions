using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace StatefulPatternFunctions.Rest
{
    public abstract class RestClientBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly IConfiguration _configuration;

        protected string _baseUrl;
        protected string _apiKey;

        public RestClientBase(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this._configuration = configuration;

            ReadConfiguration();
        }

        protected void ReadConfiguration()
        {
            var section = this._configuration.GetSection("RestClient");
            if (section == null)
                throw new Exception("Configuration is not valid. Add 'RestClient' section");

            this._baseUrl = section["BaseUrl"];
            if (string.IsNullOrWhiteSpace(this._baseUrl))
                throw new Exception("Configuration is not valid. Add 'BaseUrl' value");
            if (this._baseUrl.EndsWith("/"))
                this._baseUrl = this._baseUrl.Remove(this._baseUrl.Length - 1);

            this._apiKey = section["ApiKey"];
        }

        protected virtual Uri CreateAPIUri(string apiEndpoint)
        {
            string url;

            if (string.IsNullOrWhiteSpace(apiEndpoint))
            {
                url = $"{this._baseUrl}";
            }
            else
            {
                if (apiEndpoint.StartsWith("/"))
                    apiEndpoint = apiEndpoint.Remove(0, 1);
                url = $"{this._baseUrl}/{apiEndpoint}";
            }

            if (!string.IsNullOrWhiteSpace(this._apiKey))
            {
                if (!url.Contains("?"))
                {
                    url = $"{url}?code={this._apiKey}";
                }
                else
                {
                    url = $"{url}&code={this._apiKey}";
                }
            }
            
            return new Uri(url);
        }

        protected virtual HttpClient CreateHttpClient(string apiEndpoint)
        {
            this._httpClient.BaseAddress = CreateAPIUri(apiEndpoint);
            return this._httpClient;
        }

    }
}
