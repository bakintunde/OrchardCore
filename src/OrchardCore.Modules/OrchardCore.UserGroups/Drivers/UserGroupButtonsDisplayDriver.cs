using System.Threading.Tasks;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Users.Models;

namespace OrchardCore.UserGroups.Drivers
{
    public class UserGroupButtonsDisplayDriver : DisplayDriver<UserGroup>
    {
        public override IDisplayResult Edit(UserGroup userGroup)
        {
            return Dynamic("UserGroupSaveButtons_Edit").Location("Actions");
        }

        public override Task<IDisplayResult> UpdateAsync(UserGroup userGroup, UpdateEditorContext context)
        {
            return Task.FromResult(Edit(userGroup));
        }
    }
}
