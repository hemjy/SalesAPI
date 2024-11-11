namespace SalesAPI.Tests.Infrastructure
{
    public class DatabaseFixture : IDisposable
    {
        public PostgresDbFixture PostgresDbFixture { get; private set; }

        public DatabaseFixture()
        {
            // Initialize the fixture (PostgresDbFixture handles database creation and cleanup)
            PostgresDbFixture = new PostgresDbFixture();
        }

        public void Dispose()
        {
            // Cleanup logic if necessary
            PostgresDbFixture.Dispose();
        }
    }

}
