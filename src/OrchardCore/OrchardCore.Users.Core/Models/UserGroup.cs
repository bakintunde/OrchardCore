using System.Collections.Generic;
using OrchardCore.Entities;

namespace OrchardCore.Users.Models{
    public class UserGroup : Entity, IUserGroup
    {
        public int Id {get;set;}
        public string GroupName {get;set;}
        public int? ParentGroupId {get;set;}
        public List<int> ChildGroups { get; set; } = new List<int>();

        public override string ToString()
        {
            return GroupName;
        }
    }
}