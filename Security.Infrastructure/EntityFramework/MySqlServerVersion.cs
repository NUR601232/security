namespace Security.Infrastructure.EntityFramework
{
    internal class MySqlServerVersion
    {
        private Version version;

        public MySqlServerVersion(Version version)
        {
            this.version = version;
        }
    }
}