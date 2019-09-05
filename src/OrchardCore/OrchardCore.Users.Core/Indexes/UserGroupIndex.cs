using OrchardCore.Users.Models;
using YesSql.Indexes;

namespace OrchardCore.Users.Indexes
{
    public class UserGroupIndex : MapIndex
    {
        public int GroupId {get;set;}
        public string GroupName {get;set;}
        public int ParentGroupId {get;set;}
    }

    public class UserGroupIndexProvider : IndexProvider<UserGroup>
    {
        public override void Describe(DescribeContext<UserGroup> context)
        {
            context.For<UserGroupIndex>()
                .Map(userGroup =>
                {
                    return new UserGroupIndex
                    {
                        GroupId = userGroup.Id,
                        GroupName = userGroup.GroupName,
                        ParentGroupId = userGroup.ParentGroupId.GetValueOrDefault()
                    };
                });
        }
    }
}