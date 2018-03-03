using System;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace ContentExplorer.Helpers
{
    using Sitecore.Configuration;
    using Sitecore.Data;

    public static class ItemHelper
    {
        public static bool DoesItemHasPresentationDetails(this Item item)
        {
            if (item != null)
            {
                return item.Fields[Sitecore.FieldIDs.LayoutField] != null
                       && !String.IsNullOrEmpty(item.Fields[Sitecore.FieldIDs.LayoutField].Value);
            }
            return false;
        }

        public static string GetItemRelativeURL(this Item item)
        {
            string itemPath = item.Paths.Path.ToString().ToLower();
            itemPath = itemPath.Replace(Sitecore.Context.Data.Site.RootPath.ToLower(), "");
            itemPath = itemPath.Replace(Sitecore.Context.Data.Site.StartItem.ToLower(), "");
            return itemPath;
        }

        public static string GetIcon(this Item item)
        {
            if (!item.Fields["__Icon"].HasValue)
                return "";
            return "/temp/iconcache/" + item.Fields["__Icon"].Value;
        }

        public static string GetWorkflowState(this Item item)
        {
            var database = Factory.GetDatabase("master");
            var workflow = database.WorkflowProvider.GetWorkflow(item);
            var state = workflow?.GetState(item);
            return state != null ? state.DisplayName : string.Empty;
        }
    }
}