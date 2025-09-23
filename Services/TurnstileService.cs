using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BSLTours.API.Services
{
    public class TurnstileService : ITurnstileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;
        private readonly ILogger<TurnstileService> _logger;

        public TurnstileService(HttpClient httpClient, IConfiguration configuration, ILogger<TurnstileService> logger)
        {
            _httpClient = httpClient;
            _secretKey = configuration["Turnstile:SecretKey"] ?? throw new InvalidOperationException("Turnstile:SecretKey configuration is missing");
            _logger = logger;
        }

        public async Task<bool> VerifyTokenAsync(string token, string? remoteIp = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogWarning("Turnstile token verification failed: Token is null or empty");
                    return false;
                }

                var requestData = new Dictionary<string, string>
                {
                    ["secret"] = _secretKey,
                    ["response"] = token
                };

                if (!string.IsNullOrWhiteSpace(remoteIp))
                {
                    requestData["remoteip"] = remoteIp;
                }

                var content = new FormUrlEncodedContent(requestData);
                var response = await _httpClient.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Turnstile verification HTTP request failed with status: {StatusCode}", response.StatusCode);
                    return false;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var verificationResult = JsonSerializer.Deserialize<TurnstileVerificationResponse>(responseContent);

                if (verificationResult?.Success == true)
                {
                    _logger.LogInformation("Turnstile token verification successful");
                    return true;
                }

                _logger.LogWarning("Turnstile token verification failed. Errors: {Errors}",
                    verificationResult?.ErrorCodes != null ? string.Join(", ", verificationResult.ErrorCodes) : "Unknown");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during Turnstile token verification");
                return false;
            }
        }
    }

    public class TurnstileVerificationResponse
    {
        public bool Success { get; set; }

        public DateTime ChallengeTs { get; set; }

        public string? Hostname { get; set; }

        public string[]? ErrorCodes { get; set; }

        public string? Action { get; set; }

        public string? Cdata { get; set; }
    }
}