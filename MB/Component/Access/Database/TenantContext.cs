using MB.Utilities.AccessTokenProvider;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;

namespace MB.Access.Tenant.Database
{
    public class TenantContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly string _tenantName;

        public DbSet<Models.Message> Messages { get; set; }

        public TenantContext(
            DbContextOptions<TenantContext> options,
            IConfiguration configuration,
            IAccessTokenProvider accessTokenProvider)
            : this(options, configuration, accessTokenProvider, null)
        {
        }

        public TenantContext(
            DbContextOptions<TenantContext> options,
            IConfiguration configuration,
            IAccessTokenProvider accessTokenProvider,
            string tenantName)
            : this(options)
        {
            _configuration = configuration;
            _accessTokenProvider = accessTokenProvider;
            _tenantName = tenantName;
        }

        public TenantContext(DbContextOptions<TenantContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("tenant");

            modelBuilder.Entity<Models.Message>(entity =>
            {
                entity.HasKey(message => new { message.TenantName, message.MessageId })
                    .HasName("PK_Message");
                entity.Property(message => message.MessageId)
                    .ValueGeneratedOnAdd();
                entity.Property(message => message.TenantName)
                    .HasDefaultValueSql("CAST(SESSION_CONTEXT(N'TenantName') AS nvarchar(100))");
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_configuration == null)
            {
                // access to local DB
                base.OnConfiguring(optionsBuilder);
            }
            else
            {
                // build connection
                var connectionString = _configuration.GetConnectionString(nameof(TenantContext));
                var connection = new SqlConnection(connectionString);

                // access to azure DB using managed identity?
                if (_accessTokenProvider != null)
                {
                    // get access token
                    connection.AccessToken = _accessTokenProvider.GetSqlDbAccessToken().Result;
                }

                // tenant specific?
                if (!string.IsNullOrWhiteSpace(_tenantName))
                {
                    // open the connection first
                    connection.Open();

                    // set TenantName in SESSION_CONTEXT to enable Row-Level Security filtering
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = @"exec sp_set_session_context @key=N'TenantName', @value=@tenantName";
                    cmd.Parameters.AddWithValue("@tenantName", _tenantName);
                    cmd.ExecuteNonQuery();
                }

                optionsBuilder.UseSqlServer(
                    connection,
                    builder =>
                    {
                        /* Default sqlConnection timeout = 15 sec, our configured timeout = 10 sec
                         * 
                         * RetryExecutionStrategy defaults to
                         * MaxRetryCount = 6
                         * DefaultMaxDelay = TimeSpan.FromSeconds(30)
                         * see https://github.com/dotnet/efcore/blob/v3.1.5/src/EFCore/Storage/ExecutionStrategy.cs
                         *
                         * !Note that RetryOnFailure only applies to atomic Queries or SaveChanges() operations. Not to multiple Operations within one transaction.
                         * 
                         * We use 1 retry with a 3 seconds max delay
                         * This ensures that the sql connection is no longer racing the bus timeout
                         * When the sql connection fails both times, the sql exception will be thrown before the bus times out
                         */
                        builder.EnableRetryOnFailure(1, TimeSpan.FromSeconds(3), null);
                        builder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "tenant");
                    });
            }
        }
    }
}
