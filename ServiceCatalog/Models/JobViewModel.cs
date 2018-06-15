namespace ServiceCatalog.Models
{
    using System;

    public class JobViewModel
    {
        public string Id { get; set; }

        public string Owner { get; set; }

        public Properties Properties { get; set; }

        public string Outputs { get; set; }
    }

    public class Properties
    {
        public string JobId { get; set; }

        public RunBook RunBook { get; set; }

        public string ProvisioningState { get; set; }

        public string Status { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }

    public class RunBook
    {
        public string Name { get; set; }
    }
}