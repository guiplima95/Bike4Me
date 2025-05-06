using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Bike4Me.IntegrationTests.Abstractions;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory<Program>>;