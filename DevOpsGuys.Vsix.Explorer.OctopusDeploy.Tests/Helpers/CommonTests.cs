namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.Helpers
{
    using System;
    using System.Collections.Generic;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;

    using Machine.Specifications;

    using NUnit.Framework;

    [TestFixture]
    public class CommonTests
    {
        private IEnumerable<TestCaseData> VersionNumbersDataSource
        {
            get
            {
                yield return new TestCaseData("2.0.0", new Version("2.0.0")).SetName("2.0.0");
                yield return new TestCaseData("2.0.0-beta001", new Version("2.0.0")).SetName("2.0.0-beta001");
            }
        }

        [Test]
        [TestCaseSource("VersionNumbersDataSource")]
        public void VersionNumbersHandledCorrectly(string rawVersionNumber, Version expectedVersionNumber)
        {
            // Act
            var extractedVersionNumber = Common.GetVersionNumber(rawVersionNumber);

            // Assert
            extractedVersionNumber.Major.ShouldEqual(expectedVersionNumber.Major);
            extractedVersionNumber.Minor.ShouldEqual(expectedVersionNumber.Minor);
            extractedVersionNumber.Revision.ShouldEqual(expectedVersionNumber.Revision);
        }
    }
}
