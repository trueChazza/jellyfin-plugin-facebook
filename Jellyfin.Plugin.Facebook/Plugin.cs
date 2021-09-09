using System;
using System.Collections.Generic;
using Jellyfin.Plugin.Facebook.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Facebook
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer): base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        public override string Name => "Facebook";

        public IEnumerable<PluginPageInfo> GetPages() =>
            new[]
            {
                new PluginPageInfo
                {
                    Name = "facebook",
                    EmbeddedResourcePath = GetType().Namespace + ".Web.facebook.html",
                },
                new PluginPageInfo
                {
                    Name = "facebookjs",
                    EmbeddedResourcePath = GetType().Namespace + ".Web.facebook.js"
                }
            };

        public override string Description => "Send notifications to Facebook.";

        private readonly Guid _id = new Guid("0df52034-4b59-443f-ac40-1d3f5107e2da");
        public override Guid Id => _id;

        public static Plugin Instance { get; private set; }
    }
}
