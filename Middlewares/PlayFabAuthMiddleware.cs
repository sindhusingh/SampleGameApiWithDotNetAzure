using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SampleGameApiWithDotNetAzure.Middleware
{
    public class PlayFabAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PlayFabAuthMiddleware> _logger;

        public PlayFabAuthMiddleware(RequestDelegate next, ILogger<PlayFabAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Authorization", out var sessionTicket))
            {
                var isValid = await ValidatePlayFabSessionTicketAsync(sessionTicket);
                if (!isValid)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid PlayFab Session Ticket.");
                    return;
                }

                // Continue pipeline if ticket is valid
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("PlayFab Session Ticket is missing.");
            }
        }

        private async Task<bool> ValidatePlayFabSessionTicketAsync(string ticket)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestContent = new
                {
                    SessionTicket = ticket
                };

                var response = await client.PostAsJsonAsync("https://titleId.playfabapi.com/Client/GetAccountInfo", requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"PlayFab ticket validation failed. Status code: {response.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while validating PlayFab Session Ticket.");
                return false;
            }
        }
    }
}
