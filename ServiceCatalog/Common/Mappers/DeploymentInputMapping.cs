// -----------------------------------------------------------------------
// <copyright file="DeploymentInputMapping.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Mappers
{
    using BusinessLogic.Exceptions;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using CsvHelper.Configuration;
    using Constants;
    using DataContracts;

    public sealed class DeploymentInputMapping : CsvClassMap<DeploymentInput>
    {
        public const string PasswordRegEx =
            "(?=^.{8,20}$)((?=.*\\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|" +
            "(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*";

        public DeploymentInputMapping()
        {
            Map(m => m.ClassName).Name(DeploymentFieldConstants.ClassName);

            Map(m => m.SubscriptionName).Name(DeploymentFieldConstants.SubscriptionName);
            Map(m => m.SubscriptionId).Name(DeploymentFieldConstants.SubscriptionId);

            Map(m => m.StudentName).Name(DeploymentFieldConstants.StudentName);
            Map(m => m.StudentEmailAddress).Name(DeploymentFieldConstants.StudentEmailAddress);

            Map(m => m.VirtualMachineAdminUserName).Name(DeploymentFieldConstants.VirtualMachineAdminUserName)
                .ConvertUsing(row =>
                {
                    if (row.GetField<string>(DeploymentFieldConstants.VirtualMachineAdminUserName).Any(Char.IsWhiteSpace))
                    {
                        throw new ServiceCatalogException($"{DeploymentFieldConstants.VirtualMachineAdminUserName} cannot contain a space!");
                    }

                    return row.GetField<string>(DeploymentFieldConstants.VirtualMachineAdminUserName);
                });
            Map(m => m.VirtualMachineAdminUserNamePassword).Name(DeploymentFieldConstants.VirtualMachineAdminUserNamePassword)
                .ConvertUsing(row =>
                {
                    var password = row.GetField<string>(DeploymentFieldConstants.VirtualMachineAdminUserNamePassword);
                    Regex passwordRegex = new Regex(PasswordRegEx);
                    var match = passwordRegex.Match(password);
                    if (!match.Success)
                    {
                        throw new ServiceCatalogException($"{DeploymentFieldConstants.VirtualMachineAdminUserNamePassword} must be 8 characters including 1 uppercase letter, 1 special character, alphanumeric characters");
                    }

                    return row.GetField<string>(DeploymentFieldConstants.VirtualMachineAdminUserNamePassword);
                });

            Map(m => m.VirtualMachineUserName).Name(DeploymentFieldConstants.VirtualMachineUserName);

            Map(m => m.VirtualMachineUserNamePassword).Name(DeploymentFieldConstants.VirtualMachineUserNamePassword)
                .ConvertUsing(row =>
                {
                    var password = row.GetField<string>(DeploymentFieldConstants.VirtualMachineUserNamePassword);
                    Regex passwordRegex = new Regex(PasswordRegEx);
                    var match = passwordRegex.Match(password);
                    if (!match.Success)
                    {
                        throw new ServiceCatalogException($"{DeploymentFieldConstants.VirtualMachineUserNamePassword} must be 8 characters including 1 uppercase letter, 1 special character, alphanumeric characters");
                    }

                    return row.GetField<string>(DeploymentFieldConstants.VirtualMachineUserNamePassword);
                });

            Map(m => m.Comment).Name(DeploymentFieldConstants.Comment);
        }
    }
}