using System;
using System.Collections.Generic;
using System.Linq;
using ContentExplorer.Helpers;
using ContentExplorer.Model.JSS;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;

namespace ContentExplorer.Extensions.SC.Processors.JSS
{
    public class ContentTreeContextExtension : Sitecore.JavaScriptServices.ViewEngine.LayoutService.Pipelines.GetLayoutServiceContext.JssGetLayoutServiceContextProcessor
    {
        public const string objectKey = "contentTree";

        public ContentTreeContextExtension(Sitecore.JavaScriptServices.Configuration.IConfigurationResolver configurationResolver) : base(configurationResolver)
        {
        }

        protected override void DoProcess(GetLayoutServiceContextArgs args,
            Sitecore.JavaScriptServices.Configuration.AppConfiguration application)
        {
            Item renderedItem = args.RenderedItem;
            Database database = renderedItem != null ? renderedItem.Database : (Database)null;

            if (renderedItem == null || database == null)
                return;

            var current = new TreeItemModel
            {
                key = renderedItem.ID.ToShortID().ToString(),
                parent = renderedItem.ParentID.ToShortID().ToString(),
                title = string.IsNullOrWhiteSpace(renderedItem.DisplayName) ? renderedItem.Name : renderedItem.DisplayName,
                path = renderedItem.GetItemRelativeURL(),
                icon = "/temp/IconCache/" + renderedItem.Appearance.Icon,
                workflow = renderedItem.GetWorkflowState(),
                template = renderedItem.TemplateName
            };

            var result = new List<TreeItemModel>();
            result.Add(current);
            result.AddRange(database.SelectItems(renderedItem.Paths.FullPath + "//*")
                .Where(q => q.DoesItemHasPresentationDetails()).Select(q => new TreeItemModel
                {
                    key = q.ID.ToShortID().ToString(),
                    parent = q.ParentID.ToShortID().ToString(),
                    title = string.IsNullOrWhiteSpace(q.DisplayName) ? q.Name : q.DisplayName,
                    path = q.GetItemRelativeURL(),
                    icon = "/temp/IconCache/" + q.Appearance.Icon,
                    workflow = q.GetWorkflowState(),
                    template = q.TemplateName
                }));

            args.ContextData.Add(objectKey, result);
        }
    }
}