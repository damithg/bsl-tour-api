using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                _logger.LogInformation("Starting Turnstile verification. Token length: {TokenLength}, IP: {RemoteIp}",
                    token?.Length ?? 0, remoteIp ?? "not provided");

                if (string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogWarning("Turnstile token verification failed: Token is null or empty");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(_secretKey))
                {
                    _logger.LogError("Turnstile secret key is not configured");
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

                _logger.LogInformation("Sending verification request to Cloudflare with IP: {RemoteIp}", remoteIp ?? "none");

                var content = new FormUrlEncodedContent(requestData);
                var response = await _httpClient.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Cloudflare response status: {StatusCode}, Content: {Content}",
                    response.StatusCode, responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Turnstile verification HTTP request failed with status: {StatusCode}, Response: {Response}",
                        response.StatusCode, responseContent);
                    return false;
                }

                var verificationResult = JsonSerializer.Deserialize<TurnstileVerificationResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (verificationResult?.Success == true)
                {
                    _logger.LogInformation("Turnstile token verification successful for IP: {RemoteIp}", remoteIp);
                    return true;
                }

                _logger.LogWarning("Turnstile token verification failed. Success: {Success}, Errors: {Errors}, Full response: {Response}",
                    verificationResult?.Success,
                    verificationResult?.ErrorCodes != null ? string.Join(", ", verificationResult.ErrorCodes) : "no errors",
                    responseContent);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during Turnstile token verification. Token: {TokenPrefix}...",
                    token?.Length > 10 ? token.Substring(0, 10) : token);
                return false;
            }
        }
    }

    public class TurnstileVerificationResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime? ChallengeTs { get; set; }

        [JsonPropertyName("hostname")]
        public string? Hostname { get; set; }

        [JsonPropertyName("error-codes")]
        public string[]? ErrorCodes { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("cdata")]
        public string? Cdata { get; set; }
    }
}