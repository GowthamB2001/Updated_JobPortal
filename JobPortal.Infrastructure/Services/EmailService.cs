using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Models.Email;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JobPortal.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EmailService(
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var apiKey = _configuration["Brevo:ApiKey"];
            var senderName = _configuration["Brevo:SenderName"];
            var senderEmail = _configuration["Brevo:SenderEmail"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("Brevo API key is not configured.");

            if (string.IsNullOrWhiteSpace(senderEmail))
                throw new Exception("Brevo sender email is not configured.");

            var requestBody = new
            {
                sender = new
                {
                    name = senderName,
                    email = senderEmail
                },
                to = new[]
                {
                    new
                    {
                        email = message.To
                    }
                },
                subject = message.Subject,
                htmlContent = message.Body
            };

            var json = JsonSerializer.Serialize(requestBody);

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.brevo.com/v3/smtp/email");

            request.Headers.Add("api-key", apiKey);

            request.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            using var response = await _httpClient.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Brevo email failed. Status: {(int)response.StatusCode}, Response: {responseContent}");
            }
        }
    }
}