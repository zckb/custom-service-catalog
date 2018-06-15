// -----------------------------------------------------------------------
// <copyright file="CsvHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CsvHelper;

namespace ServiceCatalog.Common.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using DataContracts;
    using Mappers;

    public static class CsvHelper
    {
        public static List<DeploymentInput> GetDeploymentInputs(Stream stream)
        {
            var csvReader = new CsvReader(new StreamReader(stream));
            csvReader.Configuration.RegisterClassMap<DeploymentInputMapping>();

            return csvReader.GetRecords<DeploymentInput>().ToList();
        }
    }
}