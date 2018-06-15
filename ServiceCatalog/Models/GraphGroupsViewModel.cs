// -----------------------------------------------------------------------
// <copyright file="GraphGroupsViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System.ComponentModel;

    public class GraphGroupsViewModel
    {
        public string ObjectType { get; set; }

        [DisplayName("Group Id")]
        public string ObjectId { get; set; }

        public object DeletionTimestamp { get; set; }

        public string Description { get; set; }

        public object DirSyncEnabled { get; set; }

        [DisplayName("Group Name")]
        public string DisplayName { get; set; }

        public object Mail { get; set; }

        public string MailNickname { get; set; }

        public bool MailEnabled { get; set; }

        public bool SecurityEnabled { get; set; }
    }
}