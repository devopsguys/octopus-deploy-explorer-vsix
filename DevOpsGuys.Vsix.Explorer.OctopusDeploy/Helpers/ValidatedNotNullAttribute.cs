// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatedNotNullAttribute.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Attribute to ensure that CA1062 Design Rule is passed when validating using an external method.
//   HTTP://SOCIAL.MSDN.MICROSOFT.COM/FORUMS/EN-US/52D40A8E-0DAD-41E9-826A-A6FAC21B266C/INCORRECT-FIRING-OF-CA1062?FORUM=VSTSCODE.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers
{
    using System;

    /// <summary>
    /// Attribute to ensure that CA1062 Design Rule is passed when validating using an external method.
    /// HTTP://SOCIAL.MSDN.MICROSOFT.COM/FORUMS/EN-US/52D40A8E-0DAD-41E9-826A-A6FAC21B266C/INCORRECT-FIRING-OF-CA1062?FORUM=VSTSCODE.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
