using System.Collections.Generic;

namespace OrchardCore.UserGroups.ViewModels
{
    public class UserGroupIndexViewModel
    {
        public IList<UserGroupEntry> UserGroups { get; set; } = new List<UserGroupEntry>();
        // public UserGroupIndexOptions Options { get; set; }
        // public dynamic Pager { get; set; }
    }

    public class UserGroupEntry
    {
        public dynamic Shape { get; set; }
        public int GroupId {get;set;}
        public int? ParentGroupId {get;set;}
        public List<UserGroupEntry> ChildGroups {get;set;} = new List<UserGroupEntry>();
    }

    public class UserGroupIndexOptions
    {
        public string Search { get; set; }
        public UserGroupsOrder Order { get; set; }
        public UserGroupsFilter Filter { get; set; }
        public UserGroupsBulkAction BulkAction { get; set; }
    }

    public enum UserGroupsOrder
    {
        Name,
        CreatedUtc
    }

        public enum UserGroupsFilter
    {
        All,
        Approved,
        Pending,
        EmailPending
    }

    public enum UserGroupsBulkAction
    {
        None,
        Delete,
        Disable,
        Approve,
        ChallengeEmail
    }
}
