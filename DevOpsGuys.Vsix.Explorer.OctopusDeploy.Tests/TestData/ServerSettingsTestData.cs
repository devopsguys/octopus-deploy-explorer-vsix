namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.TestData
{
    using System.IO;

    using Newtonsoft.Json;

    using Octopus.Client.Model;

    /// <summary>
    /// Helper class for server settings.
    /// </summary>
    internal static class ServerSettingsTestData
    {
        internal static string SavedServerSettings()
        {
            return File.ReadAllText("TestData\\ServerSettings.json");
        }

    }
}
