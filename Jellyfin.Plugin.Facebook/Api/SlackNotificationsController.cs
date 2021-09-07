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

namespace Jellyfin.Plugin.Facebook.Api
{
    [ApiController]
    [Route("Notification/Facebook")]
    [Produces(MediaTypeNames.Application.Json)]
    public class FacebookNotificationsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FacebookNotificationsController> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public FacebookNotificationsController(ILogger<FacebookNotificationsController> logger, IHttpClientFactory httpClientFactory)
        {
              _logger = logger;
              _httpClientFactory = httpClientFactory;
              _jsonSerializerOptions = JsonDefaults.GetOptions();

        }

        /// <summary>
        /// Sends a facebook notification to test the configuration.
        /// </summary>
        /// <param name="userId">The user id of the Jellyfin user.</param>
        /// <response code="204">Notification sent successfully.</response>
        /// <returns>A <see cref="NoContentResult"/> indicating success.</returns>
        [HttpPost("Test/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SendFacebookTestNotification([FromRoute] string userId)
        {
            var options = Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.JellyfinUserId, userId, StringComparison.OrdinalIgnoreCase));

            var parameters = new Dictionary<string, string>
            {
                {"text", "This is a test notification from Jellyfin"}
            };
            if (!string.IsNullOrEmpty(options.Username))
            {
                parameters.Add("username", options.Username);
            }

            if (!string.IsNullOrEmpty(options.IconUrl))
            {
                parameters.Add("icon_url", options.IconUrl);
            }

            using var response = await _httpClientFactory.CreateClient(NamedClient.Default)
                .PostAsJsonAsync(options.WebHookUrl, parameters, _jsonSerializerOptions)
                .ConfigureAwait(false);
            _logger.LogInformation("Facebook test notification sent.");

            return NoContent();
        }
    }
}
