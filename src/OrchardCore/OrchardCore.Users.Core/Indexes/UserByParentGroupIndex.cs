using System.Linq;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Users.Models;
using YesSql.Indexes;
using System.Collections.Generic;

namespace OrchardCore.Users.Indexes
{
    public class UserByParentGroupIndex : ReduceIndex
    {
        public int GroupId { get; set; }
        public List<int> ChildGroupIds { get; set; } = new List<int>();
    }

    public class UserByParentGroupIndexProvider : IndexProvider<UserGroup>
    {
        private readonly ILookupNormalizer _keyNormalizer;

        public UserByParentGroupIndexProvider(ILookupNormalizer keyNormalizer)
        {
            _keyNormalizer = keyNormalizer;
        }

        public override void Describe(DescribeContext<UserGroup> context)
        {
            // context.For<UserByParentGroupIndex, string>()
            //     .Map(userGroup => new UserByParentGroupIndex{
            //         ParentGroupId = userGroup.ParentGroupId.GetValueOrDefault()
            //     })
            //     .Group(userGroup => userGroup.ParentGroupId)
            //     .Reduce(group => new UserByParentGroupIndex{

            //     })

                //1. select all groupIds that have this parentid
                //2. if any, recursively get all groups that have them as parentids too until none is found
                //3. concat all of them together
                // .Map(user => user.Select(x => new UserByRoleNameIndex
                // {
                //     RoleName = NormalizeKey(x),
                //     Count = 1
                // }));
                // .Group(user => user.RoleName)
                // .Reduce(group => new UserByRoleNameIndex
                // {
                //     RoleName = group.Key,
                //     Count = group.Sum(x => x.Count)
                // })
                // .Delete((index, map) =>
                // {
                //     index.Count -= map.Sum(x => x.Count);
                //     return index.Count > 0 ? index : null;
                // });
        }

        private string NormalizeKey(string key)
        {
            return _keyNormalizer == null ? key : _keyNormalizer.Normalize(key);
        }
    }
}