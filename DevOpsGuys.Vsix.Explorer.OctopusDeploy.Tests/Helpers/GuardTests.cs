// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuardTests.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Tests for the Guard helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.Helpers
{
    using System;
    
    using NUnit.Framework;

    /// <summary>
    /// Tests for the Guard helper.
    /// </summary>
    [TestFixture]
    public sealed class GuardTests
    {
        /// <summary>
        /// Ensure ArgumentIsOfType method checks for nulls on the argument.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "The parameter 'argument' cannot be null.\r\nParameter name: argument")]
        public void EnsureArgumentIsOfTypeChecksNullArgument()
        {
            // Act
            OctopusDeploy.Helpers.Guard.ArgumentIsOfType(null, null, "param");
        }

        /// <summary>
        /// Ensure ArgumentIsOfType method checks for nulls on the type.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "The parameter 'type' cannot be null.\r\nParameter name: type")]
        public void EnsureArgumentIsOfTypeChecksNullType()
        {
            // Act
            OctopusDeploy.Helpers.Guard.ArgumentIsOfType(new object(), null, "param");
        }

        /// <summary>
        /// Ensure ArgumentIsOfType method throws exception when types don't match.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "The parameter 'param' is not of type 'String'.\r\nParameter name: param")]
        public void EnsureArgumentIsOfTypeThrowsWhenTypesDiffer()
        {
            // Act
            OctopusDeploy.Helpers.Guard.ArgumentIsOfType(new object(), typeof(string), "param");
        }

        /// <summary>
        /// Validate ArgumentIsOfType method when types match.
        /// </summary>
        [Test]
        public void ValidateArgumentIsOfTypeWhenTypesMatch()
        {
            // Act
            OctopusDeploy.Helpers.Guard.ArgumentIsOfType("string", typeof(string), "param");
        }

        /// <summary>
        /// Ensure ArgumentNotNull method checks for nulls on the argument.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "The parameter 'param' cannot be null.\r\nParameter name: param")]
        public void EnsureArgumentNotNullChecksNullArgument()
        {
            // Act
            OctopusDeploy.Helpers.Guard.ArgumentNotNull(null, "param");
        }
        
        /// <summary>
        /// Ensure ArgumentNotNullOrEmpty method checks for nulls on the argument.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "The parameter 'argument' cannot be null.\r\nParameter name: argument")]
        public void EnsureArgumentNotNullOrEmptyChecksNullArgument()
        {
            // Act
            OctopusDeploy.Helpers.Guard.ArgumentNotNullOrEmpty(null, "param");
        }

        /// <summary>
        /// Ensure ArgumentNotNullOrEmpty method checks for empty strings on the argument.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "The parameter 'param' cannot be empty.\r\nParameter name: param")]
        public void EnsureArgumentNotNullOrEmptyChecksEmptyArgument()
        {
            // Act
            OctopusDeploy.Helpers.Guard.ArgumentNotNullOrEmpty(string.Empty, "param");
        }
    }
}
