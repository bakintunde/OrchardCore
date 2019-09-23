using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Security.Permissions;
using OrchardCore.Navigation;
using OrchardCore.DisplayManagement;
using OrchardCore.Users.Models;
using OrchardCore.Users.Indexes;
using OrchardCore.Data.Migration;
using YesSql.Indexes;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.UserGroups.Drivers;
using OrchardCore.Users;

namespace OrchardCore.UserGroups
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //Models
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();

            //Data
            services.AddSingleton<IIndexProvider, UserGroupIndexProvider>();
            services.AddScoped<IDataMigration, Migrations>();

            //View
            services.AddScoped<IDisplayManager<IUserGroup>, DisplayManager<IUserGroup>>();
            services.AddScoped<IDisplayDriver<IUserGroup>, UserGroupDisplayDriver>();
            services.AddScoped<IDisplayDriver<IUserGroup>, UserGroupButtonsDisplayDriver>();            

            //services
            services.AddScoped<IUserGroupService, UserGroupService>();    
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "Home",
                areaName: "OrchardCore.UserGroups",
                template: "Home/Index",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}