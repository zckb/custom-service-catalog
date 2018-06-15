// -----------------------------------------------------------------------
// <copyright file="DeploymentInput.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.DataContracts
{
    using FileHelpers;
    using System.ComponentModel;

    [DelimitedRecord(",")]
    public class DeploymentInput
    {
        [DisplayName("Class Name")]
        public string ClassName { get; set; }

        [DisplayName("Subscription Name")]
        public string SubscriptionName { get; set; }
        [DisplayName("Subscription ID")]
        public string SubscriptionId { get; set; }

        [DisplayName("Student Name")]
        public string StudentName { get; set; }
        [DisplayName("Student Email")]
        public string StudentEmailAddress { get; set; }

        [DisplayName("User Name")]
        public string VirtualMachineUserName { get; set; }
        [DisplayName("User Password")]
        public string VirtualMachineUserNamePassword { get; set; }

        [DisplayName("Admin Name")]
        public string VirtualMachineAdminUserName { get; set; }
        [DisplayName("Admin Password")]
        public string VirtualMachineAdminUserNamePassword { get; set; }

        public string Comment { get; set; }
    }
}