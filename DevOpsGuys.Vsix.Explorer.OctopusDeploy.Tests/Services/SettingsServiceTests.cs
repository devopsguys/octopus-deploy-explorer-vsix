namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.Services
{
    using System;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Tests.TestData;

    using Machine.Specifications;

    using Microsoft.VisualStudio.Settings;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Rhino.Mocks;

    /// <summary>
    /// Tests for the Settings service.
    /// </summary>
    [TestFixture]
    public class SettingsServiceTests
    {
        /// <summary>
        /// The Settings service.
        /// </summary>
        private ISettingsService service;

        /// <summary>
        /// The mocked log service.
        /// </summary>
        private ILogService logService;

        /// <summary>
        /// The mocked settings store provider.
        /// </summary>
        private ISettingsStoreProvider settingsStoreProvider;

        /// <summary>
        /// The mocked writable settings store.
        /// </summary>
        private WritableSettingsStore writableSettingsStore;
        
        /// <summary>
        /// Setup before an individual test.
        /// </summary>
        [SetUp]
        public void Set()
        {
            this.logService = MockRepository.GenerateStrictMock<ILogService>();
            this.settingsStoreProvider = MockRepository.GenerateStrictMock<ISettingsStoreProvider>();
            this.writableSettingsStore = MockRepository.GenerateStrictMock<WritableSettingsStore>();
            this.service = new SettingsService(this.logService, this.settingsStoreProvider);
        }

        /// <summary>
        /// Verify after a test has run.
        /// </summary>
        [TearDown]
        public void Verify()
        {
            this.logService.VerifyAllExpectations();
            this.settingsStoreProvider.VerifyAllExpectations();
            this.writableSettingsStore.VerifyAllExpectations();
        }

        [Test]
        public void GetSettingsWhenNoneSaved()
        {
            // Arrange
            this.settingsStoreProvider.Expect(x => x.GetWritableSettingsStore()).Return(this.writableSettingsStore);
            this.writableSettingsStore.Expect(x => x.PropertyExists(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return(false);

            // Act
            var settings = this.service.GetSettings();

            // Assert
            settings.ShouldNotBeNull();
            settings.OctopusServerUrl.ShouldBeNull();
            settings.ApiKey.ShouldBeNull();
        }

        [Test]
        public void JsonSerializationExceptionHandled()
        {
            // Arrange
            this.logService.Expect(x => x.Error(null)).IgnoreArguments();
            this.settingsStoreProvider.Expect(x => x.GetWritableSettingsStore()).Return(this.writableSettingsStore);
            this.writableSettingsStore.Expect(x => x.PropertyExists(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return(true);
            this.writableSettingsStore.Expect(x => x.GetString(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return("[]");

            // Act
            var settings = this.service.GetSettings();

            // Assert
            settings.ShouldNotBeNull();
            settings.OctopusServerUrl.ShouldBeNull();
            settings.ApiKey.ShouldBeNull();
        }

        [Test]
        public void JsonReaderExceptionHandled()
        {
            // Arrange
            this.logService.Expect(x => x.Error(null)).IgnoreArguments();
            this.settingsStoreProvider.Expect(x => x.GetWritableSettingsStore()).Return(this.writableSettingsStore);
            this.writableSettingsStore.Expect(x => x.PropertyExists(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return(true);
            this.writableSettingsStore.Expect(x => x.GetString(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return("{a}");

            // Act
            var settings = this.service.GetSettings();

            // Assert
            settings.ShouldNotBeNull();
            settings.OctopusServerUrl.ShouldBeNull();
            settings.ApiKey.ShouldBeNull();
        }

        [Test]
        public void GeneralExceptionHandled()
        {
            // Arrange
            this.logService.Expect(x => x.Error(null)).IgnoreArguments();
            this.settingsStoreProvider.Expect(x => x.GetWritableSettingsStore()).Return(this.writableSettingsStore);
            this.writableSettingsStore.Expect(x => x.PropertyExists(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return(true);
            this.writableSettingsStore.Expect(x => x.GetString(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Throw(new Exception());

            // Act
            var settings = this.service.GetSettings();

            // Assert
            settings.ShouldNotBeNull();
            settings.OctopusServerUrl.ShouldBeNull();
            settings.ApiKey.ShouldBeNull();
        }

        [Test]
        public void GetSettingsWhenSomeSaved()
        {
            // Arrange
            this.settingsStoreProvider.Expect(x => x.GetWritableSettingsStore()).Return(this.writableSettingsStore);
            this.writableSettingsStore.Expect(x => x.PropertyExists(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return(true);
            this.writableSettingsStore.Expect(x => x.GetString(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return(ServerSettingsTestData.SavedServerSettings());

            // Act
            var settings = this.service.GetSettings();

            // Assert
            settings.ShouldNotBeNull();
            settings.OctopusServerUrl.ShouldEqual("https://demo.octopusdeploy.com/");
            settings.ApiKey.ShouldEqual("AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA6PQ/PZJkjkuCNs1ytwZvwAAAAAACAAAAAAAQZgAAAAEAACAAAAAjGHW9mZBc0QLvdmNrd2piEXYuKQolo3B0r+7vPeSBIQAAAAAOgAAAAAIAACAAAADy23iR5I1PDMqGUCe/F8SvRn00sGKD2FRxVmv0M9MWfUAAAABGKmPkCqeM52QfFTuubAzrI8VOUMsTlJyvJINMFSpyLlivD4DJ8HeCmT3IZ3cW0uD+qGB7EXK0m+PG5al6fFN4QAAAAOMm+j+MrxgRZzsQN73rwTrb1eSO2Ys9McxkYFQf0pFNqzZBWr3zOw81Ejao4hbGxHXztLjPepkjl2rws+sd79s=");
        }

        [Test]
        public void CreateCollectionWhenSaved()
        {
            // Arrange
            var jsonData = ServerSettingsTestData.SavedServerSettings();          
            var newSettings = JsonConvert.DeserializeObject<ServerSettings>(jsonData);
            this.settingsStoreProvider.Expect(x => x.GetWritableSettingsStore()).Return(this.writableSettingsStore);
            this.writableSettingsStore.Expect(x => x.CollectionExists(SettingsService.SettingsCategoryName)).Return(false);
            this.writableSettingsStore.Expect(x => x.CreateCollection(SettingsService.SettingsCategoryName));
            this.writableSettingsStore.Expect(x => x.SetString(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName, jsonData));

            // Act
            this.service.SaveSettings(newSettings);
        }

        [Test]
        public void DontCreateCollectionWhenSaved()
        {
            // Arrange
            var jsonData = ServerSettingsTestData.SavedServerSettings();
            var newSettings = JsonConvert.DeserializeObject<ServerSettings>(jsonData);
            this.settingsStoreProvider.Expect(x => x.GetWritableSettingsStore()).Return(this.writableSettingsStore);
            this.writableSettingsStore.Expect(x => x.CollectionExists(SettingsService.SettingsCategoryName)).Return(true);
            this.writableSettingsStore.Expect(x => x.SetString(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName, jsonData));

            // Act
            this.service.SaveSettings(newSettings);
        }

        [Test]
        public void ReturnCachedSettingsWhenAlreadyLookedUp()
        {
            // Arrange
            this.settingsStoreProvider.Expect(x => x.GetWritableSettingsStore()).Return(this.writableSettingsStore);
            this.writableSettingsStore.Expect(x => x.PropertyExists(SettingsService.SettingsCategoryName, SettingsService.SettingsPropertyName)).Return(false);

            // Act
            var settings = this.service.GetSettings();

            // Assert
            settings.ShouldNotBeNull();
            settings.OctopusServerUrl.ShouldBeNull();
            settings.ApiKey.ShouldBeNull();

            // Arrange - CHange settings to endure we return instance and not a new one.
            settings.OctopusServerUrl = "http://w.w.com";
            settings.ApiKey = "A";

            // Act 
            settings = this.service.GetSettings();

            // Assert
            settings.ShouldNotBeNull();
            settings.OctopusServerUrl.ShouldEqual("http://w.w.com/");
            settings.ApiKey.ShouldEqual("A");
        }
    }
}
