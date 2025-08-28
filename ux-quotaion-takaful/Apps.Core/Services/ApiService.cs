using Apps.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Services
{
    public class ApiService : IApiService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private string _token;
        private readonly object _lockObject = new object();
        private bool _disposed = false;

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    // For development/testing only
                    // In production, implement proper certificate validation
                    return true;
                }
            };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Set timeout to prevent hanging requests
            _httpClient.Timeout = TimeSpan.FromSeconds(120);

        }

        public async Task<(bool Success, string Returns)> GetAsync(string url)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return (true, content);
                    }

                    return (false, $"HTTP Error: {response.StatusCode}");
                }

                /*var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return (true, content);
                }

                return (false, $"HTTP Error: {response.StatusCode}");*/
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Returns)> PostAsync(string url, HttpContent content)
        {
            try
            {
                using (var response = await _httpClient.PostAsync(url, content).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return (true, responseContent);
                    }

                    return (false, $"HTTP Error: {response.StatusCode}");
                }

                /*var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return (true, responseContent);
                }

                return (false, $"HTTP Error: {response.StatusCode}");*/
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public void SetToken(string token)
        {
            lock (_lockObject)
            {
                _token = token;
                UpdateAuthorizationHeader();
            }
        }

        /*public void SetToken(string token)
        {
            _token = token;
            UpdateAuthorizationHeader();
        }*/

        private void UpdateAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
