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
        public const string FooterNavigationKey = "footerNavigation";

        public CurrentItemContextExtension(Sitecore.JavaScriptServices.Configuration.IConfigurationResolver configurationResolver) : base(configurationResolver)
        {
        }

        protected override void DoProcess(GetLayoutServiceContextArgs args,
            Sitecore.JavaScriptServices.Configuration.AppConfiguration application)
        {
            throw new NotImplementedException();
        }
    }
}