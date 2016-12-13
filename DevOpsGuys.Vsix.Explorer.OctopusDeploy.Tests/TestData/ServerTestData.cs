// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerTestData.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Helper class for common API response scenarios.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.TestData
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    using Octopus.Client.Model;

    /// <summary>
    /// Helper class for common API response scenarios.
    /// </summary>
    internal static class ServerTestData
    {
        internal static List<ProjectGroupResource> ProjectGroups(string serverVersion)
        {
            var json = File.ReadAllText("TestData\\Responses\\" + serverVersion + "\\ProjectGroups.json");
            return JsonConvert.DeserializeObject<List<ProjectGroupResource>>(json);
        }

        internal static ResourceCollection<ProjectResource> Projects(string serverVersion, ProjectGroupResource projectGroupResource)
        {
            var json = File.ReadAllText("TestData\\Responses\\" + serverVersion + "\\" + projectGroupResource.Id + ".json");
            return JsonConvert.DeserializeObject<ResourceCollection<ProjectResource>>(json);
        }

        internal static ProjectResource Project(string serverVersion, string projectId)
        {
            var json = File.ReadAllText("TestData\\Responses\\" + serverVersion + "\\" + projectId + ".json");
            return JsonConvert.DeserializeObject<ProjectResource>(json);
        }

        internal static ProgressionResource Progresssion(string serverVersion, ProjectResource projectResource)
        {
            var json = File.ReadAllText("TestData\\Responses\\" + serverVersion + "\\" + projectResource.Id + "-Progression.json");
            return JsonConvert.DeserializeObject<ProgressionResource>(json);
        }

        internal static VariableSetResource VariableSet(string serverVersion, ProjectResource projectResource)
        {
            var json = File.ReadAllText("TestData\\Responses\\" + serverVersion + "\\" + projectResource.Id + "-VariableSet.json");
            return JsonConvert.DeserializeObject<VariableSetResource>(json);
        }

    }
}
