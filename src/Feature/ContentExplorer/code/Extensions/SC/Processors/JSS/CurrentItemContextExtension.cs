using System;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;
using Sitecore.Links;

namespace ContentExplorer.Extensions.SC.Processors.JSS
{
    public class CurrentItemContextExtension : Sitecore.JavaScriptServices.ViewEngine.LayoutService.Pipelines.GetLayoutServiceContext.JssGetLayoutServiceContextProcessor
    {
        public const string CurrentItemObjectKey = "footerNavigation";

        public CurrentItemContextExtension(Sitecore.JavaScriptServices.Configuration.IConfigurationResolver configurationResolver) : base(configurationResolver)
        {
        }

        protected override void DoProcess(GetLayoutServiceContextArgs args,
            Sitecore.JavaScriptServices.Configuration.AppConfiguration application)
        {
            Item renderedItem = args.RenderedItem;
            Database database = renderedItem != null ? renderedItem.Database : (Database)null;

            if (renderedItem == null || database == null)
                return;

            args.ContextData.Add(CurrentItemObjectKey, "works");
        }
    }
}