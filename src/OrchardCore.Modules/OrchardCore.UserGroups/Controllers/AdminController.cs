using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Settings;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using YesSql;
using OrchardCore.Users.Models;
using OrchardCore.Navigation;
using OrchardCore.UserGroups.ViewModels;
using OrchardCore.Users.Indexes;
using Microsoft.AspNetCore.Routing;

namespace OrchardCore.UserGroups.Controllers
{
    public class AdminController : Controller, IUpdateModel
    {
        private readonly IUserGroupService _userGroupService;
        private readonly ISession _session;
        private readonly IAuthorizationService _authorizationService;
        private readonly ISiteService _siteService;
        //private readonly IDisplayManager<User> _userDisplayManager;
        private readonly IDisplayManager<UserGroup> _userGroupsDisplayManager;
        private readonly INotifier _notifier;
        //private readonly IUserService _userService;

        private readonly dynamic New;
        private readonly IHtmlLocalizer TH;

        public AdminController(
                IAuthorizationService authorizationService,
                ISession session,
                IDisplayManager<UserGroup>  userGroupDisplayManager,
                IUserGroupService userGroupService,
                INotifier notifier,
                ISiteService siteService,
                IShapeFactory shapeFactory,
                IHtmlLocalizer<AdminController> htmlLocalizer
                )
        {
            _userGroupsDisplayManager = userGroupDisplayManager;
            _authorizationService = authorizationService;
            _session = session;
            _userGroupService = userGroupService;
            _notifier = notifier;
            _siteService = siteService;            

            New = shapeFactory;
            TH = htmlLocalizer;
        }

        public async Task<ActionResult> Index(UserGroupIndexOptions options, PagerParameters pagerParameters)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageUserGroups))
            {
                return Unauthorized();
            }

            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var pager = new Pager(pagerParameters, siteSettings.PageSize);

            // default options
            if (options == null)
            {
                options = new UserGroupIndexOptions();
            }

            var userGroups = _session.Query<UserGroup, UserGroupIndex>();

            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                userGroups = userGroups.Where(u => u.GroupName.Contains(options.Search));
            }

            var count = await userGroups.CountAsync();

            var results = await userGroups
                .Skip(pager.GetStartIndex())
                .Take(pager.PageSize)
                .ListAsync();

            // Maintain previous route data when generating page links
            var routeData = new RouteData();
            routeData.Values.Add("Options.Filter", options.Filter);
            routeData.Values.Add("Options.Search", options.Search);
            routeData.Values.Add("Options.Order", options.Order);

            var pagerShape = (await New.Pager(pager)).TotalItemCount(count).RouteData(routeData);

            var userGroupEntries = new List<UserGroupEntry>();

            foreach (var userGroup in results)
            {
                userGroupEntries.Add(new UserGroupEntry
                {
                    Shape = await _userGroupsDisplayManager.BuildDisplayAsync(userGroup, updater: this, displayType: "SummaryAdmin")
                });
            }

            var model = new UserGroupIndexViewModel
            {
                UserGroups = userGroupEntries,
                Options = options,
                Pager = pagerShape
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageUserGroups))
            {
                return Unauthorized();
            }

            var shape = await _userGroupsDisplayManager.BuildEditorAsync(new UserGroup(), updater: this, isNew: true);

            return View(shape);
        }


        [HttpPost]
        [ActionName(nameof(Create))]
        public async Task<IActionResult> CreatePost()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageUserGroups))
            {
                return Unauthorized();
            }

            var userGroup = new UserGroup();

            var shape = await _userGroupsDisplayManager.UpdateEditorAsync(userGroup, updater: this, isNew: true);

            if (!ModelState.IsValid)
            {
                return View(shape);
            }

            await _userGroupService.CreateUserGroupAsync(userGroup);

            if (!ModelState.IsValid)
            {
                return View(shape);
            }

            _notifier.Success(TH["User group created successfully"]);

            return RedirectToAction(nameof(Index));
        }
    }
}
