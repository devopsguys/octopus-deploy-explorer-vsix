// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Guard.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Helper to validate external parameters.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Helper to validate external parameters.
    /// </summary>
    internal static class Guard
    {
        /// <summary>
        /// Ensure the argument is of a certain type.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentException">The argument is not the correct type.</exception>
        public static void ArgumentIsOfType([ValidatedNotNull] object argument, [ValidatedNotNull] Type type, string parameterName)
        {
            ArgumentNotNull(argument, "argument");
            ArgumentNotNull(type, "type");

            if (!type.IsInstanceOfType(argument))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The parameter '{0}' is not of type '{1}'.", parameterName, type.Name), parameterName);
            }
        }

        /// <summary>
        /// Ensure the argument is not null.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentNullException">The argument is null.</exception>
        public static void ArgumentNotNull([ValidatedNotNull] object argument, string parameterName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName, string.Format(CultureInfo.CurrentCulture, "The parameter '{0}' cannot be null.", parameterName));
            }
        }

        /// <summary>
        /// Ensure the argument is not null or empty.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentException">The argument is empty.</exception>
        public static void ArgumentNotNullOrEmpty([ValidatedNotNull] string argument, string parameterName)
        {
            ArgumentNotNull(argument, "argument");

            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The parameter '{0}' cannot be empty.", parameterName), parameterName);
            }
        }
    }
}