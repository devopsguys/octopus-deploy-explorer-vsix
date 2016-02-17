namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests
{
    using Octopus.Client.Model;

    internal static class Extensions
    {
        internal static DashboardItemResource ToDashboardItemResource(this object item)
        {
            return (DashboardItemResource)item;
        }
    }
}
