// Guids.cs
// MUST match guids.h
using System;

namespace DevOpsGuys.octopus_deploy_vsix
{
    static class GuidList
    {
        public const string guidoctopus_deploy_vsixPkgString = "75658403-61b4-48db-b625-a4f30b71621f";
        public const string guidoctopus_deploy_vsixCmdSetString = "11152db3-8242-4dfe-b692-47c89fa33141";
        public const string guidToolWindowPersistanceString = "8860adbf-7b5f-4a8d-93b4-a5096ff8f1ab";

        public static readonly Guid guidoctopus_deploy_vsixCmdSet = new Guid(guidoctopus_deploy_vsixCmdSetString);
    };
}