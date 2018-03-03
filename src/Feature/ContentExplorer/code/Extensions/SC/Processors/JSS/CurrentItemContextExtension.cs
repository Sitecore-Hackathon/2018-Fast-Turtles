using System;
using System.Linq;
using ContentExplorer.Model.JSS;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;
using Sitecore.Links;
using Sitecore.Workflows;

namespace ContentExplorer.Extensions.SC.Processors.JSS
{
    public class CurrentItemContextExtension : Sitecore.JavaScriptServices.ViewEngine.LayoutService.Pipelines.GetLayoutServiceContext.JssGetLayoutServiceContextProcessor
    {
        public const string CurrentItemObjectKey = "currentItem";

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

            var outputObject = new CurrentItemModel
            {
                CreatedBy = renderedItem.Statistics.CreatedBy,
                UpdatedBy = renderedItem.Statistics.UpdatedBy,
                CountOfVersions = renderedItem.Versions.Count,
                CurrentVersion = renderedItem.Version.Number,
                IsPublished = !renderedItem.Publishing.NeverPublish,
                WorkflowState = String.Empty
            };

            //IWorkflow workflow = database.WorkflowProvider.GetWorkflow(renderedItem);
            //WorkflowState state = workflow.GetState(renderedItem);
            //outputObject.WorkflowState = state.DisplayName;

            args.ContextData.Add(CurrentItemObjectKey, outputObject);
        }
    }
}