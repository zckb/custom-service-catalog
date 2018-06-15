// -----------------------------------------------------------------------
// <copyright file="TemplateHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Helpers
{
    using System.IO;
    using Constants;
    using DataContracts;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using BusinessLogic.Exceptions;
    using Models;

    public static class TemplateHelper
    {
        public static string ToJson(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, Formatting.Indented, settings);
        }

        public static JObject GetJsonFileContents(string pathToJson)
        {
            pathToJson.AssertNotEmpty(nameof(pathToJson));

            using (var file = new StreamReader(pathToJson))
            {
                using (var reader = new JsonTextReader(file))
                {
                    var templatefileContent = (JObject)JToken.ReadFrom(reader);

                    return templatefileContent;
                }
            }
        }

        public static JObject PrepareTemplateParametersWithValues(string templateJson, Dictionary<string, string> parametersDictionary)
        {
            var template = JObject.Parse(templateJson);

            var parametersSection = template[TemplateConstants.ParametersSection];
            if (parametersSection == null)
            {
                throw new ServiceCatalogException($"Couldn't find '{TemplateConstants.ParametersSection}' section in JSON template.");
            }

            foreach (var parameterKvp in parametersDictionary)
            {
                if (!(parametersSection[parameterKvp.Key] is JObject))
                {
                    throw new ServiceCatalogException($"Couldn't find parameter '{parameterKvp.Key}' in JSON template");
                }

                parametersSection[parameterKvp.Key] = new JObject();
                parametersSection[parameterKvp.Key][TemplateConstants.ValueSection] = new JValue(parameterKvp.Value);
            }

            return parametersSection.ToObject<JObject>();
        }

        public static JObject PrepareParameters(DeploymentInput deploymentInput, DeploymentOutput deploymentOutput, string path)
        {
            deploymentInput.AssertNotNull(nameof(deploymentInput));
            deploymentOutput.AssertNotNull(nameof(deploymentOutput));
            path.AssertNotEmpty(nameof(path));

            var parameterFileContents = TemplateHelper.GetJsonFileContents(path);

            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.AdminUserName][TemplateConstants.ValueSection] = new JValue(deploymentInput.VirtualMachineAdminUserName);
            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.AdminPassword][TemplateConstants.ValueSection] = new JValue(deploymentInput.VirtualMachineAdminUserNamePassword);

            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.UserName][TemplateConstants.ValueSection] = new JValue(deploymentInput.VirtualMachineUserName);
            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.UserPassword][TemplateConstants.ValueSection] = new JValue(deploymentInput.VirtualMachineUserNamePassword);

            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.DomainNameLabel][TemplateConstants.ValueSection] = new JValue(deploymentOutput.VirtualMachineDomainName);
            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.VirtualMachineName][TemplateConstants.ValueSection] = new JValue(deploymentOutput.VirtualMachineName);

            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.TagClassName][TemplateConstants.ValueSection] = new JValue(deploymentOutput.ClassName);
            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.TagClassId][TemplateConstants.ValueSection] = new JValue(deploymentOutput.ClassId);

            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.TagStudentEmail][TemplateConstants.ValueSection] = new JValue(deploymentOutput.StudentEmailAddress);

            parameterFileContents[TemplateConstants.ParametersSection][TemplateConstants.TagStudentName][TemplateConstants.ValueSection] = new JValue(deploymentOutput.StudentName);

            return parameterFileContents;
        }

        public static List<JsonTemplateParameter> ReadParametersFromTemplateJson(string templateJson)
        {
            var parsedJsonObject = JObject.Parse(templateJson);
            var parametersProperty = parsedJsonObject.Property(TemplateConstants.ParametersSection)?.Value;
            if (parametersProperty == null)
            {
                throw new ServiceCatalogException($"Couldn't locate '{TemplateConstants.ParametersSection}' property in JSON.");
            }

            return parametersProperty.Select(ParseJsonParameterRecord).ToList();
        }

        private static JsonTemplateParameter ParseJsonParameterRecord(JToken parameterJsonToken)
        {
            var parameterJsonProperty = parameterJsonToken as JProperty;
            var name = parameterJsonProperty?.Name;
            var parameterValueJsonObject = parameterJsonProperty?.Value as JObject;
            var type = (parameterValueJsonObject?.Property("type").Value as JValue)?.Value as string;
            var allowedValues = (parameterValueJsonObject?.Property("allowedValues")?.Value as JArray)?.Values<string>().ToList();
            var defaultValue = (parameterValueJsonObject?.Property("defaultValue")?.Value as JValue)?.Value as string;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type))
            {
                throw new ServiceCatalogException("Invalid input parameter format in JSON.");
            }

            var template = new JsonTemplateParameter
            {
                Name = name,
                Type = type == "string"
                    ? JsonTemplateParameterType.String
                    : type == "securestring"
                        ? JsonTemplateParameterType.SecureString
                        : JsonTemplateParameterType.Unknown,
                AllowedValues = allowedValues,
                DefaultValue = defaultValue
            };

            return template;
        }
    }
}