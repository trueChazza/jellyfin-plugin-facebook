using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Facebook.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public FacebookConfiguration[] Options { get; set; }

        public PluginConfiguration()
        {
            Options = new FacebookConfiguration[] { };
        }
    }
}
