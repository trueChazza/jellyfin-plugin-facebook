using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using MediaBrowser.Common.Json;
using MediaBrowser.Common.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jellyfin.Plugin.Facebook.Configuration;

namespace Jellyfin.Plugin.Facebook.Api
{
    [ApiController]
    [Route("Notification/Facebook")]
    [Produces(MediaTypeNames.Application.Json)]
    public class NotificationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NotificationController> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public NotificationController(ILogger<NotificationController> logger, IHttpClientFactory httpClientFactory)
        {
              _logger = logger;
              _httpClientFactory = httpClientFactory;
              _jsonSerializerOptions = JsonDefaults.GetOptions();

        }

        [HttpPost("Test")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SendTestNotification()
        {
            var parameters = new Dictionary<string, string>
            {
                {
                    "message", "This is a test notification from Jellyfin"
                }
            };

            using var response = await _httpClientFactory.CreateClient(NamedClient.Default)
                .PostAsJsonAsync($"https://graph.facebook.com/v11.0/{ Plugin.Instance.Configuration.GroupId }/feed?access_token={ Plugin.Instance.Configuration.AccessToken }", parameters, _jsonSerializerOptions)
                .ConfigureAwait(false);

            _logger.LogInformation("Facebook test notification sent.");

            return NoContent();
        }
    }
}
