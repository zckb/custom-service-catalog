// -----------------------------------------------------------------------
// <copyright file="UriConstants.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Constants
{
    public static class UriConstants
    {
        public const string GetAllSubscriptionsUri = "/subscriptions?api-version=2016-09-01";

        public const string GetResourceGroupUri = "/subscriptions/{0}/resourcegroups/{1}?api-version=2016-09-01";

        public const string GetAllResourceGroupsUri = "/subscriptions/{0}/resourcegroups?api-version=2016-09-01{1}";

        public const string CreateResourceGroupUri = "/subscriptions/{0}/resourcegroups/{1}?api-version=2016-09-01";

        public const string DeleteResourceGroupUri = "/subscriptions/{0}/resourcegroups/{1}?api-version=2016-09-01";

        public const string GetAllLocationsUri = "/subscriptions/{0}/locations?api-version=2017-05-10";

        public const string GetCurrentComputeUsageInformation = "/subscriptions/{0}/providers/Microsoft.Compute/locations/{1}/usages?api-version=2016-04-30-preview";

        public const string GetCurrentNetworkInformation = "/subscriptions/{0}/providers/Microsoft.Network/locations/{1}/usages?api-version=2016-04-30-preview";

        // Lists the virtual machines in a subscription | GET
        public const string GetAllVirtualMachineUri = "/subscriptions/{0}/providers/Microsoft.Compute/virtualmachines?api-version=2016-04-30-preview";

        // Start | POST
        public const string StartVirtualMachineUri = "/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachines/{2}/start?api-version=2016-04-30-preview";

        // Stop | POST
        public const string StopVirtualMachineUri = "/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachines/{2}/stop?api-version=2016-04-30-preview";

        // Delete VM | DELETE
        public const string DeleteVirtualMachineUri = "/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachines/{2}?api-version=2016-04-30-preview";

        // Create Vm via template | PUT
        public const string CreateDeploymentsUri = "/subscriptions/{0}/resourcegroups/{1}/providers/Microsoft.Resources/deployments/{2}?api-version=2016-09-01";

        // Get Deploy state | GET
        public const string GetDeploymentsUri = "/subscriptions/{0}/resourcegroups/{1}/providers/Microsoft.Resources/deployments/{2}?api-version=2016-09-01";

        // Check Dns Name Availability
        public const string CheckDnsNameAvailabilityUri = "/subscriptions/{0}/providers/Microsoft.Network/locations/{1}/CheckDnsNameAvailability?api-version=2017-03-01&domainNameLabel={2}";

        // Get all the deployments for a resource group.
        public const string GetDeploymentsByResourceGroup = "/subscriptions/{0}/resourcegroups/{1}/providers/Microsoft.Resources/deployments/?api-version=2017-05-10";

        // Get all the available invoices for a subscription
        public const string GetInvoices = "/subscriptions/{0}/providers/Microsoft.Billing/invoices?api-version=2017-04-24-preview&$expand=downloadUrl";

        // Get all automation accounts
        public const string GetAutomationAccounts = "/subscriptions/{0}/providers/Microsoft.Automation/automationAccounts?api-version=2015-10-31";
        
        // Get all jobs by automation accounts
        public const string GetJobs = "{0}/jobs?api-version=2015-10-31";

        // Get Job output
        public const string GetJobOutput = "{0}/output?api-version=2015-10-31";

        // Gets a collection of groups.
        public const string GetGraphGroups = "https://graph.windows.net/myorganization/groups?api-version=1.6";

        // Gets a collection of users.
        public const string GetGraphUsers = "https://graph.windows.net/myorganization/users?api-version=1.6";

        // ARM Vizualize site Url
        public const string ArmVizualizeUrl = "http://armviz.io/#/?load={0}";

        // Content type
        public static string JsonContentType = "application/json";
    }
}
