// -----------------------------------------------------------------------
// <copyright file="Template.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog
{
    public class Template
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsManage { get; set; }
        public string Version { get; set; }
        public string UsersGroup { get; set; }
    }
}
