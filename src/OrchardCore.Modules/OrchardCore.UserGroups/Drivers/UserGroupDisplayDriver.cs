using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Security.Services;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using OrchardCore.UserGroups.ViewModels;
using OrchardCore.Users.ViewModels;

namespace OrchardCore.UserGroups.Drivers
{
    public class UserGroupDisplayDriver : DisplayDriver<UserGroup>
    {
        private readonly IStringLocalizer T;
        public UserGroupDisplayDriver(
            IStringLocalizer<UserGroupDisplayDriver> stringLocalizer)
        {
            T = stringLocalizer;
        }

        public override IDisplayResult Display(UserGroup userGroup)
        {
            return Combine(
                Initialize<SummaryAdminUserGroupViewModel>("UserGroupFields", model => model.UserGroup = userGroup).Location("SummaryAdmin", "Header:1"),
                Initialize<SummaryAdminUserGroupViewModel>("UserGroupButtons", model => model.UserGroup = userGroup).Location("SummaryAdmin", "Actions:1")
            );
        }

        public override Task<IDisplayResult> EditAsync(UserGroup userGroup, BuildEditorContext context)
        {
            return Task.FromResult<IDisplayResult>(Initialize<EditUserGroupViewModel>("UserGroupFields_Edit", model =>
            {
                model.Id = userGroup.Id;
                model.GroupName = userGroup.GroupName;
            }).Location("Content:1"));
        }

        public override async Task<IDisplayResult> UpdateAsync(UserGroup userGroup, UpdateEditorContext context)
        {
            var model = new EditUserGroupViewModel();

            if (!await context.Updater.TryUpdateModelAsync(model, Prefix))
            {
                return await EditAsync(userGroup, context);
            }

            if (string.IsNullOrWhiteSpace(model.GroupName)){
                context.Updater.ModelState.AddModelError("GroupName", T["A group name is required."]);
            }

            userGroup.GroupName = model.GroupName;
            return await EditAsync(userGroup, context);
        }
    }
}
