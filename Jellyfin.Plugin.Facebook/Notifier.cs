using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Facebook.Configuration;
using Jellyfin.Data.Entities;
using MediaBrowser.Common.Json;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Facebook
{
    public class Notifier : INotificationService
    {
        private readonly ILogger<Notifier> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public Notifier(ILogger<Notifier> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = JsonDefaults.GetOptions();
        }

        public bool IsEnabledForUser(User user)
        {
            return Plugin.Instance.Configuration.IsEnabled;
        }

        public string Name => Plugin.Instance.Name;

        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, string>
                {
                    {
                        "message", $"{request.Name} \n {request.Description}"
                    },
                    {
                        "link", $"{request.Url}"
                    },
                };

            _logger.LogDebug("Notification to Facebook : {0} - {1}", Plugin.Instance.Configuration.GroupId, request.Description);

            using var response = await _httpClientFactory.CreateClient(NamedClient.Default)
                .PostAsJsonAsync($"https://graph.facebook.com/v11.0/{ Plugin.Instance.Configuration.GroupId }/feed", parameters, _jsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
