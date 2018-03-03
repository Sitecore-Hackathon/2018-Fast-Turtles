﻿using System;
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

            var allItems =
                database.SelectItems(renderedItem.Paths.FullPath + "//*").Where(q => q.DoesItemHasPresentationDetails()).Select(q => new
                {
                    itemID = q.ID.ToShortID().ToString(),
                    parentID = q.ParentID.ToShortID().ToString(),
                    title = string.IsNullOrWhiteSpace(q.DisplayName) ? q.Name : q.DisplayName,
                    path = q.GetItemRelativeURL(),
                    icon = q.GetIcon()
                });



            args.ContextData.Add(objectKey, allItems);
        }
    }
}