namespace ContentExplorer.Extensions.SC.Processors.JSS
{
    using ContentExplorer.Helpers;
    using ContentExplorer.Model.JSS;

    using Sitecore.Data;
    using Sitecore.JavaScriptServices.Configuration;
    using Sitecore.JavaScriptServices.ViewEngine.LayoutService.Pipelines.GetLayoutServiceContext;
    using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;

    public class CurrentItemContextExtension : JssGetLayoutServiceContextProcessor
    {
        public const string CurrentItemObjectKey = "currentItem";

        public CurrentItemContextExtension(IConfigurationResolver configurationResolver)
            : base(configurationResolver) { }

        protected override void DoProcess(GetLayoutServiceContextArgs args, AppConfiguration application)
        {
            var renderedItem = args.RenderedItem;
            var database = renderedItem?.Database;

            if (renderedItem == null || database == null)
            {
                return;
            }

            var outputObject = new CurrentItemModel
                                   {
                                       ItemId = renderedItem.ID.ToString(),
                                       Name = renderedItem.Name,
                                       DisplayName = renderedItem.DisplayName,
                                       Path = renderedItem.Paths.Path,
                                       TemplateName = renderedItem.TemplateName,
                                       CreatedBy = renderedItem.Statistics.CreatedBy,
                                       UpdatedBy = renderedItem.Statistics.UpdatedBy,
                                       CountOfVersions = renderedItem.Versions.Count,
                                       CurrentVersion = renderedItem.Version.Number,
                                       IsPublished = !renderedItem.Publishing.NeverPublish,
                                       WorkflowState = renderedItem.GetWorkflowState()
                                   };

            args.ContextData.Add(CurrentItemObjectKey, outputObject);
        }
    }
}