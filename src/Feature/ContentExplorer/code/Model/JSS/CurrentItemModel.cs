namespace ContentExplorer.Model.JSS
{
    public class CurrentItemModel
    {
        public int CountOfVersions { get; set; }

        public string CreatedBy { get; set; }

        public int CurrentVersion { get; set; }

        public string DisplayName { get; set; }

        public bool IsPublished { get; set; }

        public string ItemId { get; set; }

        public string ItemOwner { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string TemplateName { get; set; }

        public string UpdatedBy { get; set; }

        public string WorkflowState { get; set; }
    }
}