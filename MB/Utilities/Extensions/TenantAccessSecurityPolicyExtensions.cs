using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Utilities.Extensions
{
    public static class TenantAccessSecurityPolicyExtensions
    {
        public static void AddTenantAccessSecuritySupport(this MigrationBuilder builder, string tableNameWithSchema)
        {
            string policyNameSuffix = tableNameWithSchema.Replace(".", "_");

            builder.Sql(
                $@"CREATE SECURITY POLICY security.tenantAccessPolicy_{policyNameSuffix}
                       ADD FILTER PREDICATE Security.fn_tenantAccessPredicate(TenantName) ON {tableNameWithSchema},
                       ADD BLOCK  PREDICATE security.fn_tenantAccessPredicate(TenantName) ON {tableNameWithSchema} AFTER INSERT
                       WITH(STATE = ON);");
        }

        public static void RemoveTenantAccessSecuritySupport(this MigrationBuilder builder, string tableNameWithSchema)
        {
            string policyNameSuffix = tableNameWithSchema.Replace(".", "_");

            builder.Sql(
                $@"DROP SECURITY POLICY security.tenantAccessPolicy_{policyNameSuffix};");
        }
    }
}
