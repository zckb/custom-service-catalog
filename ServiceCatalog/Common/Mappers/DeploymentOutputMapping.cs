// -----------------------------------------------------------------------
// <copyright file="DeploymentOutputMapping.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Mappers
{
    using CsvHelper.Configuration;
    using Constants;
    using DataContracts;

    public sealed class DeploymentOutputMapping : CsvClassMap<DeploymentOutput>
    {
        public DeploymentOutputMapping()
        {
            Map(m => m.ClassName).Name(DeploymentFieldConstants.ClassName);
            Map(m => m.ClassId).Name(DeploymentFieldConstants.ClassId);
            Map(m => m.SubscriptionName).Name(DeploymentFieldConstants.SubscriptionName);
            Map(m => m.SubscriptionId).Name(DeploymentFieldConstants.SubscriptionId);
            Map(m => m.StudentName).Name(DeploymentFieldConstants.StudentName);
            Map(m => m.StudentEmailAddress).Name(DeploymentFieldConstants.StudentEmailAddress);
            Map(m => m.VirtualMachineAdminUserName).Name(DeploymentFieldConstants.VirtualMachineAdminUserName);
            Map(m => m.VirtualMachineUserName).Name(DeploymentFieldConstants.VirtualMachineUserName);
            Map(m => m.VirtualMachineName).Name(DeploymentFieldConstants.VirtualMachineName);
            Map(m => m.VirtualMachineId).Name(DeploymentFieldConstants.VirtualMachineId);
            Map(m => m.VirtualMachineDomainName).Name(DeploymentFieldConstants.VirtualMachineDomainName);
            Map(m => m.ResourceGroupName).Name(DeploymentFieldConstants.ResourceGroupName);
            Map(m => m.Location).Name(DeploymentFieldConstants.Location);
            Map(m => m.Comment).Name(DeploymentFieldConstants.Comment);
        }
    }
}