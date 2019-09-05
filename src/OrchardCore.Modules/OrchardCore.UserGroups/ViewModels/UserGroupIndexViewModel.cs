using System.Collections.Generic;

namespace OrchardCore.UserGroups.ViewModels
{
    public class UserGroupIndexViewModel
    {
        public IList<UserGroupEntry> UserGroups { get; set; }
        public UserGroupIndexOptions Options { get; set; }
        public dynamic Pager { get; set; }
    }

    public class UserGroupEntry
    {
        public dynamic Shape { get; set; }
        public bool IsChecked { get; set; }
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
