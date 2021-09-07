namespace Jellyfin.Plugin.Facebook.Configuration
{
    public class FacebookConfiguration
    {
        public string WebHookUrl { get; set; }
        public bool IsEnabled { get; set; }
        public string JellyfinUserId { get; set; }
        public string Username { get; set; }
        public string IconUrl { get; set; }
    }
}
