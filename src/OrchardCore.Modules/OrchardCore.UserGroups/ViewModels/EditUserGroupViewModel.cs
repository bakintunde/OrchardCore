using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Users.ViewModels
{
    public class EditUserGroupViewModel
    {
        public int Id { get; set; }

        [Required]
        public string GroupName { get; set; }
        public List<GroupEntry> ChildGroups { get; set; } = new List<GroupEntry>();
    }

    public class GroupEntry{
        public string GroupName {get;set;}
    }
}
