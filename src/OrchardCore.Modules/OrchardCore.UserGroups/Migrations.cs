using OrchardCore.Data.Migration;
using OrchardCore.Users.Indexes;

namespace OrchardCore.UserGroups
{
    public class Migrations : DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(UserGroupIndex), table => table
                .Column<int>("GroupId")
                .Column<string>("GroupName")
                .Column<int>("ParentGroupId")
            );

            return 1;
        }
    }
}