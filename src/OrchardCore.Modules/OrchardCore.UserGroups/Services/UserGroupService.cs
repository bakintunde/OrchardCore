using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OrchardCore.Users;
using OrchardCore.Users.Indexes;
using OrchardCore.Users.Models;
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

    public async Task<IUserGroup> FindByIdAsync(int id)
    {
        var userGroup = await _session.GetAsync<UserGroup>(id);
        return userGroup;
    }

    public Task<IUserGroup> CreateUserGroupAsync(IUserGroup userGroup)
    {
        _session.Save(userGroup);
        return Task.FromResult(userGroup);
    }

    Task IUserGroupService.DeleteUserGroupAsync(IUserGroup userGroup)
    {
        _session.Delete(userGroup);
        return Task.FromResult(userGroup);
    }

    Task<IUserGroup> IUserGroupService.GetUserGroupAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<IEnumerable<IUserGroup>> IUserGroupService.GetUserGroupsAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    Task<IUserGroup> IUserGroupService.UpdateUserGroupAsync(IUserGroup userGroup)
    {
        _session.Save(userGroup);
        return Task.FromResult(userGroup);
    }
}