using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using OrchardCore.Modules;

namespace OrchardCore.UserGroups
{
    public class AdminMenu : INavigationProvider
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder
                .Add(T["Configuration"], configuration => configuration
                    .Add(T["Security"], "5", security => security
                        .Add(T["User Groups"], "5", installed => installed
                            .Action("Index", "Admin", "OrchardCore.UserGroups")
                            .Permission(Permissions.ManageUserGroups)
                            .LocalNav()
                         )));

            return Task.CompletedTask;
        }
    }
}
