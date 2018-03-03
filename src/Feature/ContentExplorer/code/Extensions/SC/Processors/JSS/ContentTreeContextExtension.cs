namespace ContentExplorer.Extensions.SC.Processors.JSS
{
    using System.Collections.Generic;
    using System.Linq;

    using ContentExplorer.Helpers;
    using ContentExplorer.Model.JSS;

    using Sitecore.Data;
    using Sitecore.JavaScriptServices.Configuration;
    using Sitecore.JavaScriptServices.ViewEngine.LayoutService.Pipelines.GetLayoutServiceContext;
    using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;

    public class ContentTreeContextExtension : JssGetLayoutServiceContextProcessor
    {
        public const string objectKey = "contentTree";

        public ContentTreeContextExtension(IConfigurationResolver configurationResolver)
            : base(configurationResolver) { }

        protected override void DoProcess(GetLayoutServiceContextArgs args, AppConfiguration application)
        {
            var renderedItem = args.RenderedItem;
            var database = renderedItem?.Database;

            if (renderedItem == null || database == null)
            {
                return;
            }

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

            var result = new List<TreeItemModel> { current };
            result.AddRange(
                database.SelectItems(renderedItem.Paths.FullPath + "//*").Where(q => q.DoesItemHasPresentationDetails()).Select(
                    q => new TreeItemModel
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