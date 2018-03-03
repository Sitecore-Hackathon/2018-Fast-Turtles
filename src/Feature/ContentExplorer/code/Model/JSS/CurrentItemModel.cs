﻿namespace ContentExplorer.Model.JSS
{
    public class CurrentItemModel
    {
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsPublished { get; set; }
        public string WorkflowState { get; set; }
        public int CountOfVersions { get; set; }
        public int CurrentVersion { get; set; }
    }
}