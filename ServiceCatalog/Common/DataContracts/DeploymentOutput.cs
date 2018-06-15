// -----------------------------------------------------------------------
// <copyright file="DeploymentOutput.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.DataContracts
{
    using FileHelpers;

    [DelimitedRecord(",")]
    public class DeploymentOutput
    {
        public string ClassName { get; set; }
        public string ClassId { get; set; }

        public string SubscriptionName { get; set; }
        public string SubscriptionId { get; set; }

        public string StudentName { get; set; }
        public string StudentEmailAddress { get; set; }

        public string VirtualMachineUserName { get; set; }
        public string VirtualMachineAdminUserName { get; set; }

        public string VirtualMachineName { get; set; }
        public string VirtualMachineId { get; set; }

        public string VirtualMachineDomainName { get; set; }
        public string Location { get; set; }

        public string ResourceGroupName { get; set; }

        public string Comment { get; set; }
    }
}