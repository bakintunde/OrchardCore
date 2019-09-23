using System.Collections.Generic;

namespace OrchardCore.Users
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserGroup
    {        
        int Id {get;set;}
        string GroupName { get; set; }
        int? ParentGroupId {get;set;}
        List<int> ChildGroups { get; set; }
    }
}
