namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.Converters
{
    using System.Collections.Generic;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Converters;

    using Machine.Specifications;

    using NUnit.Framework;

    using Octopus.Client.Model;

    [TestFixture]
    public class ReleaseVersionBackgroundConverterTests
    {
        private IEnumerable<TestCaseData> ReleaseDataSource
        {
            get
            {
                yield return new TestCaseData(new DashboardItemResource { State = TaskState.Queued }, "#335d84").SetName("Queued");
                yield return new TestCaseData(new DashboardItemResource { State = TaskState.Executing }, "#55ab55").SetName("Executing");
                yield return new TestCaseData(new DashboardItemResource { State = TaskState.TimedOut }, "#dedd84").SetName("TimedOut");
                yield return new TestCaseData(new DashboardItemResource { State = TaskState.Success }, "#55ab55").SetName("Success");
                yield return new TestCaseData(new DashboardItemResource { State = TaskState.Success, HasWarningsOrErrors = true }, "#e4a747").SetName("Success with warnings");
                yield return new TestCaseData(new DashboardItemResource { State = TaskState.Failed }, "#e45847").SetName("Failed");
                yield return new TestCaseData(new DashboardItemResource { State = TaskState.Canceled }, "#b8b8b8").SetName("Canceled");
            }
        }

        [Test]
        public void ReleaseItemConverterHandlesNull()
        {
            // Act
            var converted = new ReleaseVersionBackgroundConverter().Convert(null, typeof(DashboardItemResource), null, null);

            // Assert
            converted.ShouldBeNull();
        }

        [Test]
        [TestCaseSource("ReleaseDataSource")]
        public void ReleaseItemConvertedToColour(DashboardItemResource releaseitem, string expectedColour)
        {
            // Act
            var converted = new ReleaseVersionBackgroundConverter().Convert(releaseitem, typeof(DashboardItemResource), null, null);

            // Assert
            converted.ToString().ShouldEqual(expectedColour);
        }

        [Test]
        public void RevertBackReturnsNull()
        {
           new ReleaseVersionBackgroundConverter().ConvertBack(null, null, null, null).ShouldBeNull();
        }
    }
}
