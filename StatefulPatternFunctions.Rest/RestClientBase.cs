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
        }

        protected virtual Uri CreateAPIUri(string apiEndpoint)
        {
            if (string.IsNullOrWhiteSpace(apiEndpoint))
                return new Uri($"{this._baseUrl}");

            if (apiEndpoint.StartsWith("/"))
                apiEndpoint = apiEndpoint.Remove(0, 1);
            return new Uri($"{this._baseUrl}/{apiEndpoint}");
        }

        protected virtual HttpClient CreateHttpClient(string apiEndpoint)
        {
            this._httpClient.BaseAddress = CreateAPIUri(apiEndpoint);
            return this._httpClient;
        }

    }
}
