namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.Services
{
    using System;
    using System.Security;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.UI;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;

    using Funq;

    using Machine.Specifications;

    using NUnit.Framework;

    using Rhino.Mocks;

    /// <summary>
    /// Tests for the API client factory.
    /// </summary>
    [TestFixture]
    public class ApiClientFactoryTests
    {
        /// <summary>
        /// The API client factory.
        /// </summary>
        private IApiClientFactory apiClientFactory;

        /// <summary>
        /// The mocked log service.
        /// </summary>
        private ILogService logService;

        /// <summary>
        /// An event aggregator.
        /// </summary>
        private IEventAggregator eventAggregator;

        /// <summary>
        /// The mocked settings service.
        /// </summary>
        private ISettingsService settingsService;

        /// <summary>
        /// The mocked command service.
        /// </summary>
        private ICommandService commandService;

        /// <summary>
        /// Setup before an individual test.
        /// </summary>
        [SetUp]
        public void Set()
        {
            this.logService = MockRepository.GenerateStrictMock<ILogService>();
            this.logService.Expect(x => x.Trace(string.Empty)).IgnoreArguments().Repeat.AtLeastOnce();
            this.eventAggregator = new EventAggregator();
            this.settingsService = MockRepository.GenerateStrictMock<ISettingsService>();
            this.commandService = MockRepository.GenerateStrictMock<ICommandService>();

            var container = new Container();
            container.Register(x => this.eventAggregator);
            container.Register(this.settingsService);
            container.Register(this.commandService);
            OctopusDeployContainerPackage.Container = container;

            this.apiClientFactory = new ApiClientFactory(this.logService);
        }

        /// <summary>
        /// Verify after a test has run.
        /// </summary>
        [TearDown]
        public void Verify()
        {
            this.logService.VerifyAllExpectations();
            this.settingsService.VerifyAllExpectations();
            this.commandService.VerifyAllExpectations();
        }
        
        /// <summary>
        /// No connection details handled.
        /// </summary>
        [Test]
        public void NoConnectionDetailsHandled()
        {
            // Arrange
            this.logService.Expect(x => x.Warn(string.Empty)).IgnoreArguments();
            var model = new OctopusViewModel();
            this.settingsService.Expect(x => x.GetSettings()).Return(new ServerSettings());
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);
            
            // Act
            var client = this.apiClientFactory.GetClient();

            // Assert
            client.ShouldBeNull();
            model.ShouldNotBeNull();
            model.ExceptionMessage.ShouldStartWith(OctopusDeploy.Resources.ExceptionStartMessage);
            model.ExceptionMessage.ShouldContain(Environment.NewLine + Environment.NewLine);
            model.ExceptionMessage.ShouldEndWith(OctopusDeploy.Resources.ODE1001);
            model.ExceptionMessageButtonText.ShouldEqual(OctopusDeploy.Resources.EditSettingsButtonText);
            model.ExceptionResolutionCommand.ShouldBeOfExactType<DelegateCommand>();
        }

        /// <summary>
        /// Connection details handled.
        /// </summary>
        [Test]
        public void ConnectionDetailsHandled()
        {
            // Arrange
            this.logService.Expect(x => x.Info(string.Empty)).IgnoreArguments();
            var model = new OctopusViewModel();
            var password = new SecureString();
            password.AppendChar('a');
            this.settingsService.Expect(x => x.GetSettings()).Return(new ServerSettings { OctopusServerUrl = "https://demo.octopusdeploy.com", ApiKey = password.ToEncryptedString() });
            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(x => model = x);

            // Act
            var client = this.apiClientFactory.GetClient();

            // Assert
            client.ShouldNotBeNull();
            model.ShouldNotBeNull();
            model.ExceptionMessage.ShouldBeNull();
            model.ExceptionMessageButtonText.ShouldBeNull();
            model.ExceptionResolutionCommand.ShouldBeNull();
        }
    }
}
