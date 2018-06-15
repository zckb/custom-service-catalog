// -----------------------------------------------------------------------
// <copyright file="IDeploymentValidation.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.DataContracts;

    public interface IDeploymentValidation
    {
        Task<bool> IsValid(Dictionary<string, List<DeploymentInput>> inputDictionary);
    }
}