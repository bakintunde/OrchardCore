using System.Threading.Tasks;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Users;
using OrchardCore.Users.Models;

namespace OrchardCore.UserGroups.Drivers
{
    public class UserGroupButtonsDisplayDriver : DisplayDriver<IUserGroup>
    {
        public override IDisplayResult Edit(IUserGroup userGroup)
        {
            return Dynamic("UserGroupSaveButtons_Edit").Location("Actions");
        }

        public override Task<IDisplayResult> UpdateAsync(IUserGroup userGroup, UpdateEditorContext context)
        {
            return Task.FromResult(Edit(userGroup));
        }
    }
}
