using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Facebook.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public bool IsEnabled { get; set; }
        public string GroupId { get; set; }
        public string AccessToken { get; set; }
    }
}
