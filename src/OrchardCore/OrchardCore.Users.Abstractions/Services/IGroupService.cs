using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OrchardCore.Users;

public interface IUserGroupService
{
    Task<IUserGroup> CreateUserGroupAsync(IUserGroup userGroup);
    Task<IEnumerable<IUserGroup>> GetUserGroupsAsync(CancellationToken cancellationToken = default);    
    Task<IUserGroup> GetUserGroupAsync(int id);
    Task DeleteUserGroupAsync(IUserGroup userGroup);
    Task<IUserGroup> UpdateUserGroupAsync(IUserGroup userGroup);
    Task<IUserGroup> FindByIdAsync(int id);

}