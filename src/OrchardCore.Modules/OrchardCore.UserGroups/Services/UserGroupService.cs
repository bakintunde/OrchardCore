using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OrchardCore.Users;
using YesSql;

public class UserGroupService : IUserGroupService
{
    private readonly ISession _session;
    public UserGroupService(
        ISession session
    )
    {
        _session = session;
    }

    Task<IUserGroup> IUserGroupService.CreateUserGroupAsync(IUserGroup userGroup)
    {
        _session.Save(userGroup);
        return Task.FromResult(userGroup);
    }

    Task IUserGroupService.DeleteUserGroupAsync(string GroupName)
    {
        throw new System.NotImplementedException();
    }

    Task<IUserGroup> IUserGroupService.GetUserGroupAsync(string GroupName)
    {
        throw new System.NotImplementedException();
    }

    Task<IEnumerable<IUserGroup>> IUserGroupService.GetUserGroupsAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    Task<IUserGroup> IUserGroupService.UpdateUserGroupAsync(IUserGroup userGroup)
    {
        throw new System.NotImplementedException();
    }
}