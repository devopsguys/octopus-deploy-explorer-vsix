namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.Services
{
    using System;
    using System.Linq;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.TestData;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;

    using Machine.Specifications;

    using NUnit.Framework;

    using Octopus.Client;
    using Octopus.Client.Exceptions;
    using Octopus.Client.Model;

    using Rhino.Mocks;

    /// <summary>
    /// Tests for the API service.
    /// </summary>
    [TestFixture]
    public class ApiServiceTests
    {
        /// <summary>
        /// The V26 version number.
        /// </summary>
        private const string V26VersionNumber = "2.6.0.778";

        /// <summary>
        /// The V32 version number.
        /// </summary>
        private const string V32VersionNumber = "3.2.23";

        /// <summary>
        /// The V33 beta version number.
        /// </summary>
        private const string V33BetaVersionNumber = "3.3.0-beta0001";

        /// <summary>
        /// The API service.
        /// </summary>
        private IApiService service;

        /// <summary>
        /// The mocked log service.
        /// </summary>
        private ILogService logService;

        /// <summary>
        /// An event aggregator.
        /// </summary>
        private IEventAggregator eventAggregator;

        /// <summary>
        /// The mocked API client factory.
        /// </summary>
        private IApiClientFactory apiClientFactory;

        /// <summary>
        /// The mocked octopus client.
        /// </summary>
        private IOctopusClient octopusClient;

        /// <summary>
        /// Setup before an individual test.
        /// </summary>
        [SetUp]
        public void Set()
        {
            this.logService = MockRepository.GenerateMock<ILogService>();
            this.eventAggregator = new EventAggregator();
            this.apiClientFactory = MockRepository.GenerateStrictMock<IApiClientFactory>();
            this.octopusClient = MockRepository.GenerateStrictMock<IOctopusClient>();
            this.service = new ApiService(this.logService, this.eventAggregator, this.apiClientFactory);
        }

        /// <summary>
        /// Verify after a test has run.
        /// </summary>
        [TearDown]
        public void Verify()
        {
            this.logService.VerifyAllExpectations();
            this.apiClientFactory.VerifyAllExpectations();
            this.octopusClient.VerifyAllExpectations();
        }

        /// <summary>
        /// Null log service raises an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLogRaisesException()
        {
            // Act
            this.service = new ApiService(null, null, null);
        }

        /// <summary>
        /// Null aggregator raises an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullAggregatorRaisesException()
        {
            // Act
            this.service = new ApiService(this.logService, null, null);
        }

        /// <summary>
        /// Null client factory raises an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullClientRaisesException()
        {
            // Act
            this.service = new ApiService(this.logService, this.eventAggregator, null);
        }

        /// <summary>
        /// Unable to connect handled.
        /// </summary>
        [Test]
        public void ConnectionExceptionHandled()
        {
            // Arrange
            var model = new OctopusViewModel();
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Throw(new Exception("Unable to connect to the Octopus Deploy server.", new Exception("Inner Exception")));
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);
            
            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.ExceptionMessage.ShouldStartWith(OctopusDeploy.Resources.ExceptionStartMessage);
            model.ExceptionMessage.ShouldContain(Environment.NewLine + Environment.NewLine);
            model.ExceptionMessage.ShouldContain(OctopusDeploy.Resources.ODE1002);
            model.ExceptionMessage.ShouldEndWith("Inner Exception");
            model.ExceptionMessageButtonText.ShouldBeNull();
        }

        /// <summary>
        /// Unexpected exception handled.
        /// </summary>
        [Test]
        public void UnexpectedExceptionHandled()
        {
            // Arrange
            var model = new OctopusViewModel();
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Throw(new Exception(string.Empty));
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.ExceptionMessage.ShouldStartWith(OctopusDeploy.Resources.ExceptionStartMessage);
            model.ExceptionMessage.ShouldContain(Environment.NewLine + Environment.NewLine);
            model.ExceptionMessage.ShouldEndWith(OctopusDeploy.Resources.ODE1006);
            model.ExceptionMessageButtonText.ShouldBeNull();
        }

        /// <summary>
        /// We only support version 2.6 and above.
        /// </summary>
        [Test]
        public void MinimumVersionBelowSupported()
        {
            // Arrange
            var model = new OctopusViewModel();
            var rootDocument = new RootResource { Version = "2.3.1" };
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.ExceptionMessage.ShouldStartWith(OctopusDeploy.Resources.ExceptionStartMessage);
            model.ExceptionMessage.ShouldContain(Environment.NewLine + Environment.NewLine);
            model.ExceptionMessage.ShouldEndWith(OctopusDeploy.Resources.ODE1003);
            model.ExceptionMessageButtonText.ShouldBeNull();
        }

        /// <summary>
        /// We support prerelease version numbers too.
        /// </summary>
        [Test]
        public void PrereleaseReleaseVersionSupported()
        {
            // Arrange
            var model = new OctopusViewModel();
            var rootDocument = new RootResource { Version = V33BetaVersionNumber };
            rootDocument.Links.Add("ProjectGroups", string.Empty);
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.List<ProjectGroupResource>(rootDocument.Links["ProjectGroups"])).Return(ServerTestData.ProjectGroups(rootDocument.Version));
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.ExceptionMessage.ShouldBeNull();
            model.ExceptionMessageButtonText.ShouldBeNull();
        }

        /// <summary>
        /// Security exception handled.
        /// </summary>
        [Test]
        public void SecurityExceptionHandled()
        {
            // Arrange
            var model = new OctopusViewModel();
            var rootDocument = new RootResource { Version = V26VersionNumber };
            rootDocument.Links.Add("ProjectGroups", string.Empty);
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.List<ProjectGroupResource>(rootDocument.Links["ProjectGroups"])).Throw(new OctopusSecurityException(401, string.Empty));
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.ExceptionMessage.ShouldStartWith(OctopusDeploy.Resources.ExceptionStartMessage);
            model.ExceptionMessage.ShouldContain(Environment.NewLine + Environment.NewLine);
            model.ExceptionMessage.ShouldEndWith(OctopusDeploy.Resources.ODE1004);
            model.ExceptionMessageButtonText.ShouldEqual(OctopusDeploy.Resources.EditSettingsButtonText);
        }

        /// <summary>
        /// Deserialization exception handled.
        /// </summary>
        [Test]
        public void DeserializationExceptionHandled()
        {
            // Arrange
            var model = new OctopusViewModel();
            var rootDocument = new RootResource { Version = V26VersionNumber };
            rootDocument.Links.Add("ProjectGroups", string.Empty);
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.List<ProjectGroupResource>(rootDocument.Links["ProjectGroups"])).Throw(new OctopusDeserializationException(500, string.Empty));
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.ExceptionMessage.ShouldStartWith(OctopusDeploy.Resources.ExceptionStartMessage);
            model.ExceptionMessage.ShouldContain(Environment.NewLine + Environment.NewLine);
            model.ExceptionMessage.ShouldEndWith(OctopusDeploy.Resources.ODE1005);
            model.ExceptionMessageButtonText.ShouldEqual(OctopusDeploy.Resources.EditSettingsButtonText);
        }

        /// <summary>
        /// V26 groups request handled.
        /// </summary>
        [Test]
        public void HandleV26Groups()
        {
            // Arrange
            var model = new OctopusViewModel();
            var rootDocument = new RootResource { Version = V26VersionNumber };
            rootDocument.Links.Add("ProjectGroups", string.Empty);
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.List<ProjectGroupResource>(rootDocument.Links["ProjectGroups"])).Return(ServerTestData.ProjectGroups(rootDocument.Version));
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.Groups.Count.ShouldEqual(7);
            model.Groups[0].Resource.Id.ShouldEqual("ProjectGroups-1");
            model.Groups[0].Resource.Name.ShouldEqual("All Projects");
            model.Groups[1].Resource.Id.ShouldEqual("ProjectGroups-97");
            model.Groups[1].Resource.Name.ShouldEqual("Configuration");
            model.Groups[2].Resource.Id.ShouldEqual("ProjectGroups-66");
            model.Groups[2].Resource.Name.ShouldEqual("Databases");
            model.Groups[3].Resource.Id.ShouldEqual("ProjectGroups-129");
            model.Groups[3].Resource.Name.ShouldEqual("Integration Services");
            model.Groups[4].Resource.Id.ShouldEqual("ProjectGroups-65");
            model.Groups[4].Resource.Name.ShouldEqual("Reports");
            model.Groups[5].Resource.Id.ShouldEqual("ProjectGroups-34");
            model.Groups[5].Resource.Name.ShouldEqual("Services");
            model.Groups[6].Resource.Id.ShouldEqual("ProjectGroups-33");
            model.Groups[6].Resource.Name.ShouldEqual("Sites");
            model.ExceptionMessage.ShouldBeNull();
            model.ExceptionMessageButtonText.ShouldBeNull();
        }

        /// <summary>
        /// V26 projects request handled.
        /// </summary>
        [Test]
        public void HandleV26Projects()
        {
            // Arrange
            var groups = ServerTestData.ProjectGroups(V26VersionNumber);
            var group = new OctopusGroupViewModel(groups.Items.First(g => g.Name == "Services"));
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.List<ProjectResource>(group.Resource.Links["Projects"])).Return(ServerTestData.Projects(V26VersionNumber, group.Resource));

            // Act
            this.service.GetProjectsIntoGroup(group);

            // Assert
            group.ShouldNotBeNull();
            group.Children.Count.ShouldEqual(2);
            group.Children[0].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[1].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children.OfType<OctopusProjectViewModel>().First().Resource.Id.ShouldEqual("projects-2");
            group.Children.OfType<OctopusProjectViewModel>().First().Resource.Name.ShouldEqual("RoutingService");
            group.Children.OfType<OctopusProjectViewModel>().Last().Resource.Id.ShouldEqual("projects-3");
            group.Children.OfType<OctopusProjectViewModel>().Last().Resource.Name.ShouldEqual("Service");
        }

        /// <summary>
        /// V26 matrix request handled.
        /// </summary>
        [Test]
        public void HandleV26Matrix()
        {
            // Arrange
            var rootDocument = new RootResource { Version = V26VersionNumber };
            var project = new OctopusProjectViewModel(null, ServerTestData.Project(rootDocument.Version, "Projects-2"));
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.Get<ProgressionResource>(project.Resource.Links["Progression"])).Return(ServerTestData.Progresssion(rootDocument.Version, project.Resource));

            // Act
            this.service.GetProjectMatrix(project);

            // Assert
            project.ReleaseMatrix.ShouldNotBeNull();
            project.ReleaseMatrix.Environments.Count.ShouldEqual(5);
            project.ReleaseMatrix.Environments[0].Name.ShouldEqual("Development");
            project.ReleaseMatrix.Environments[1].Name.ShouldEqual("Test");
            project.ReleaseMatrix.Environments[2].Name.ShouldEqual("PreProd");
            project.ReleaseMatrix.Environments[3].Name.ShouldEqual("Production");
            project.ReleaseMatrix.Environments[4].Name.ShouldEqual("Laptop");
            project.ReleaseMatrix.Releases.Count.ShouldEqual(6);
        }

        /// <summary>
        /// V32 groups request handled.
        /// </summary>
        [Test]
        public void HandleV32Groups()
        {
            // Arrange
            var model = new OctopusViewModel();
            var rootDocument = new RootResource { Version = V32VersionNumber };
            rootDocument.Links.Add("ProjectGroups", string.Empty);
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.List<ProjectGroupResource>(rootDocument.Links["ProjectGroups"])).Return(ServerTestData.ProjectGroups(rootDocument.Version));
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.Groups.Count.ShouldEqual(4);
            model.Groups[0].Resource.Id.ShouldEqual("ProjectGroups-1");
            model.Groups[0].Resource.Name.ShouldEqual("All Projects");
            model.Groups[1].Resource.Id.ShouldEqual("ProjectGroups-61");
            model.Groups[1].Resource.Name.ShouldEqual("Channels");
            model.Groups[2].Resource.Id.ShouldEqual("ProjectGroups-22");
            model.Groups[2].Resource.Name.ShouldEqual("OctoFX");
            model.Groups[3].Resource.Id.ShouldEqual("ProjectGroups-40");
            model.Groups[3].Resource.Name.ShouldEqual("Other Samples");
            model.ExceptionMessage.ShouldBeNull();
            model.ExceptionMessageButtonText.ShouldBeNull();
        }

        /// <summary>
        /// V32 projects request handled.
        /// </summary>
        [Test]
        public void HandleV32Projects()
        {
            // Arrange
            var groups = ServerTestData.ProjectGroups(V32VersionNumber);
            var group = new OctopusGroupViewModel(groups.Items.First(g => g.Name == "Other Samples"));
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.List<ProjectResource>(group.Resource.Links["Projects"])).Return(ServerTestData.Projects(V32VersionNumber, group.Resource));

            // Act
            this.service.GetProjectsIntoGroup(group);

            // Assert
            group.ShouldNotBeNull();
            group.Children.Count.ShouldEqual(4);
            group.Children[0].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[1].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[2].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[3].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children.OfType<OctopusProjectViewModel>().First().Resource.Id.ShouldEqual("Projects-141");
            group.Children.OfType<OctopusProjectViewModel>().First().Resource.Name.ShouldEqual("Azure Web Blue-Green Sample");
            group.Children.OfType<OctopusProjectViewModel>().Last().Resource.Id.ShouldEqual("Projects-82");
            group.Children.OfType<OctopusProjectViewModel>().Last().Resource.Name.ShouldEqual("Output Variables");
        }

        /// <summary>
        /// V32 matrix request handled.
        /// </summary>
        [Test]
        public void HandleV32Matrix()
        {
            // Arrange
            var rootDocument = new RootResource { Version = V32VersionNumber };
            var project = new OctopusProjectViewModel(null, ServerTestData.Project(V32VersionNumber, "Projects-82"));
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.Get<ProgressionResource>(project.Resource.Links["Progression"])).Return(ServerTestData.Progresssion(V32VersionNumber, project.Resource));

            // Act
            this.service.GetProjectMatrix(project);

            // Assert
            project.ReleaseMatrix.ShouldNotBeNull();
            project.ReleaseMatrix.Environments.Count.ShouldEqual(9);
            project.ReleaseMatrix.Releases.Count.ShouldEqual(2);
            project.ReleaseMatrix.Environments[0].Name.ShouldEqual("Development");
            project.ReleaseMatrix.Environments[1].Name.ShouldEqual("Dev");
            project.ReleaseMatrix.Environments[2].Name.ShouldEqual("Test");
            project.ReleaseMatrix.Environments[3].Name.ShouldEqual("Acceptance");
            project.ReleaseMatrix.Environments[4].Name.ShouldEqual("Beta");
            project.ReleaseMatrix.Environments[5].Name.ShouldEqual("Production");
            project.ReleaseMatrix.Environments[6].Name.ShouldEqual("Production-AU");
            project.ReleaseMatrix.Environments[7].Name.ShouldEqual("Production-US");
            project.ReleaseMatrix.Environments[8].Name.ShouldEqual("Azure");
            project.ReleaseMatrix.Releases[0][0].ToDashboardItemResource().ReleaseVersion.ShouldEqual("0.0.3");
            project.ReleaseMatrix.Releases[0][0].ToDashboardItemResource().State.ShouldEqual(TaskState.Success);
            project.ReleaseMatrix.Releases[0][0].ToDashboardItemResource().HasWarningsOrErrors.ShouldBeFalse();
            project.ReleaseMatrix.Releases[0][1].ToDashboardItemResource().ReleaseVersion.ShouldEqual("0.0.3");
            project.ReleaseMatrix.Releases[0][1].ToDashboardItemResource().State.ShouldEqual(TaskState.Success);
            project.ReleaseMatrix.Releases[0][1].ToDashboardItemResource().HasWarningsOrErrors.ShouldBeTrue();
            project.ReleaseMatrix.Releases[0][2].ShouldBeNull();
            project.ReleaseMatrix.Releases[0][3].ShouldBeNull();
            project.ReleaseMatrix.Releases[0][4].ShouldBeNull();
            project.ReleaseMatrix.Releases[0][5].ShouldBeNull();
            project.ReleaseMatrix.Releases[0][6].ShouldBeNull();
            project.ReleaseMatrix.Releases[0][7].ShouldBeNull();
            project.ReleaseMatrix.Releases[0][8].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][0].ToDashboardItemResource().ReleaseVersion.ShouldEqual("0.0.2");
            project.ReleaseMatrix.Releases[1][0].ToDashboardItemResource().State.ShouldEqual(TaskState.Success);
            project.ReleaseMatrix.Releases[1][0].ToDashboardItemResource().HasWarningsOrErrors.ShouldBeFalse();
            project.ReleaseMatrix.Releases[1][1].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][2].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][3].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][4].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][5].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][6].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][7].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][8].ShouldBeNull();
        }

        /// <summary>
        /// V33Beta groups request handled.
        /// </summary>
        [Test]
        public void HandleV33BetaGroups()
        {
            // Arrange
            var model = new OctopusViewModel();
            var rootDocument = new RootResource { Version = V33BetaVersionNumber };
            rootDocument.Links.Add("ProjectGroups", string.Empty);
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.List<ProjectGroupResource>(rootDocument.Links["ProjectGroups"])).Return(ServerTestData.ProjectGroups(rootDocument.Version));
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            this.service.GetGroups();

            // Assert
            model.ShouldNotBeNull();
            model.Groups.Count.ShouldEqual(1);
            model.Groups[0].Resource.Id.ShouldEqual("ProjectGroups-1");
            model.Groups[0].Resource.Name.ShouldEqual("All Projects");
            model.ExceptionMessage.ShouldBeNull();
            model.ExceptionMessageButtonText.ShouldBeNull();
        }

        /// <summary>
        /// V33Beta projects request handled.
        /// </summary>
        [Test]
        public void HandleV33BetaProjects()
        {
            // Arrange
            var groups = ServerTestData.ProjectGroups(V33BetaVersionNumber);
            var group = new OctopusGroupViewModel(groups.Items.First(g => g.Name == "All Projects"));
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.List<ProjectResource>(group.Resource.Links["Projects"])).Return(ServerTestData.Projects(V33BetaVersionNumber, group.Resource));

            // Act
            this.service.GetProjectsIntoGroup(group);

            // Assert
            group.ShouldNotBeNull();
            group.Children.Count.ShouldEqual(7);
            group.Children[0].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[1].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[2].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[3].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[4].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[5].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children[6].ShouldBeOfExactType<OctopusProjectViewModel>();
            group.Children.OfType<OctopusProjectViewModel>().First().Resource.Id.ShouldEqual("Projects-24");
            group.Children.OfType<OctopusProjectViewModel>().First().Resource.Name.ShouldEqual("Project with latest deployment successful");
            group.Children.OfType<OctopusProjectViewModel>().Last().Resource.Id.ShouldEqual("Projects-25");
            group.Children.OfType<OctopusProjectViewModel>().Last().Resource.Name.ShouldEqual("Project with no successful deployments");
        }

        /// <summary>
        /// V33Beta matrix request handled.
        /// </summary>
        [Test]
        public void HandleV33BetaMatrix()
        {
            // Arrange
            var rootDocument = new RootResource { Version = V33BetaVersionNumber };
            var project = new OctopusProjectViewModel(null, ServerTestData.Project(V33BetaVersionNumber, "Projects-27"));
            this.apiClientFactory.Expect(x => x.GetClient()).Return(this.octopusClient);
            this.octopusClient.Expect(x => x.RootDocument).Return(rootDocument);
            this.octopusClient.Expect(x => x.Get<ProgressionResource>(project.Resource.Links["Progression"])).Return(ServerTestData.Progresssion(V33BetaVersionNumber, project.Resource));

            // Act
            this.service.GetProjectMatrix(project);

            // Assert
            project.ReleaseMatrix.ShouldNotBeNull();
            project.ReleaseMatrix.Environments.Count.ShouldEqual(2);
            project.ReleaseMatrix.Environments[0].Name.ShouldEqual("local");
            project.ReleaseMatrix.Environments[1].Name.ShouldEqual("Prod");
            project.ReleaseMatrix.Releases.Count.ShouldEqual(2);
            project.ReleaseMatrix.Releases[0][0].ToDashboardItemResource().ReleaseVersion.ShouldEqual("0.0.2");
            project.ReleaseMatrix.Releases[0][1].ShouldBeNull();
            project.ReleaseMatrix.Releases[1][0].ToDashboardItemResource().ReleaseVersion.ShouldEqual("0.0.1");
            project.ReleaseMatrix.Releases[1][1].ToDashboardItemResource().ReleaseVersion.ShouldEqual("0.0.1");
        }
    }
}
