// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctopusModelBuiltEvent.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Event for when the octopus model has been retrieved and built by the service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Events
{
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;

    /// <summary>
    /// Event for when the octopus model has been retrieved and built by the service.
    /// </summary>
    /// <seealso cref="CompositePresentationEvent{OctopusViewModel}" />
    internal class OctopusModelBuiltEvent : CompositePresentationEvent<OctopusViewModel>
    {
    }
}
