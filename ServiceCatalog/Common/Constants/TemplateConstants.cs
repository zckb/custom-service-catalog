// -----------------------------------------------------------------------
// <copyright file="TemplateConstants.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Constants
{
    public class TemplateConstants
    {
        public static string ParametersJson = "parameters.json";

        public static string TemplateJson = "template.json";


        public static string ParametersSection = "parameters";

        public static string ValueSection = "value";
        

        public static string AdminUserName = "adminUserName";

        public static string AdminPassword = "adminPassword";

        public static string UserName = "userName";

        public static string UserPassword = "userPassword";


        public static string DomainNameLabel = "dnsPrefix";

        public static string TagClassName = "tagClassName";

        public static string TagClassId = "tagClassID";

        public static string TagStudentEmail = "tagStudentEmail";

        public static string TagStudentName = "tagStudentName"; 

        public static string VirtualMachineName = "virtualMachineName";
        
        public static string DomainNameLabelTemplate = "{0}.{1}.cloudapp.azure.com";
    }
}