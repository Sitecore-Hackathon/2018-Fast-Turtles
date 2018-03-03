using System;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace ContentExplorer.Helpers
{
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
            return ThemeManager.GetIconImage(Sitecore.Context.Item, 32, 32, "", "");
        }
    }
}