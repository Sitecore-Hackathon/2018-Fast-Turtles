using System.Collections.Generic;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Serialization.Pipelines.GetFieldSerializer;

namespace ContentExplorer.Extensions.SC
{
    public class ItemSerializer : Sitecore.LayoutService.Serialization.ItemSerializers.DefaultItemSerializer
    {
        public ItemSerializer(IGetFieldSerializerPipeline getFieldSerializerPipeline)
            : base(getFieldSerializerPipeline)
        {
            this.Fields = new List<string>();
        }

        public List<string> Fields { get; }

        protected override bool FieldFilter(Field field)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            return !field.Name.StartsWith("__") || Fields.Contains(field.Name);
        }
    }
}