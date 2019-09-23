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
using OrchardCore.Users;

namespace OrchardCore.UserGroups.Controllers
{
    public class AdminController : Controller, IUpdateModel
    {
        private readonly IUserGroupService _userGroupService;
        private readonly ISession _session;
        private readonly IAuthorizationService _authorizationService;
        private readonly ISiteService _siteService;        
        private readonly IDisplayManager<IUserGroup> _userGroupsDisplayManager;
        private readonly INotifier _notifier;
        //private readonly IUserService _userService;

        private readonly dynamic New;
        private readonly IHtmlLocalizer TH;

        public AdminController(
                IAuthorizationService authorizationService,
                ISession session,
                IDisplayManager<IUserGroup> userGroupDisplayManager,
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

            // if (!string.IsNullOrWhiteSpace(options.Search))
            // {
            //     userGroups = userGroups.Where(u => u.GroupName.Contains(options.Search));
            // }

            var count = await userGroups.CountAsync();
        
            var results = await userGroups
                //.Skip(pager.GetStartIndex())
                //.Take(pager.PageSize)
                .ListAsync();
            
            // // Maintain previous route data when generating page links
            // var routeData = new RouteData();
            // routeData.Values.Add("Options.Filter", options.Filter);
            // routeData.Values.Add("Options.Search", options.Search);
            // routeData.Values.Add("Options.Order", options.Order);

            //var pagerShape = (await New.Pager(pager)).TotalItemCount(count).RouteData(routeData);

            var userGroupEntries = new List<UserGroupEntry>();            

            foreach (var userGroup in results)
            {
                UserGroupEntry uge = new UserGroupEntry
                {
                    GroupId = userGroup.Id,    
                    ParentGroupId = userGroup.ParentGroupId,                
                    Shape = await _userGroupsDisplayManager.BuildDisplayAsync(userGroup, updater: this, displayType: "SummaryAdmin")                    
                };
                               
                userGroupEntries.Add(uge);
            }
            var model  = SetupHierarchy(userGroupEntries);
            // var model = new UserGroupIndexViewModel
            // {
            //     UserGroups = userGroupEntries               
            // };

            return View(model);
        }

        public UserGroupIndexViewModel SetupHierarchy(IList<UserGroupEntry> groupEntries){
            UserGroupIndexViewModel model = new UserGroupIndexViewModel();
            foreach(var entry in groupEntries){
                if (!entry.ParentGroupId.HasValue){
                    model.UserGroups.Add(entry); // top-level groups
                }
                else{
                    //child-level group
                    groupEntries.Single(x => x.GroupId == entry.ParentGroupId.GetValueOrDefault()).ChildGroups.Add(entry);
                }
            }
            return model;
        }


        public async Task<IActionResult> Create(int? parentGroupId = null)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageUserGroups))
            {
                return Unauthorized();
            }

            IShape shape = null; 
            if (parentGroupId == null){
                shape = await _userGroupsDisplayManager.BuildEditorAsync(new UserGroup(), updater: this, isNew: true);
            }
            else{
                var userGroup = await _userGroupService.FindByIdAsync(parentGroupId.GetValueOrDefault()) as UserGroup;
                if (userGroup == null)
                {
                    return NotFound();
                }
                
                shape = await _userGroupsDisplayManager.BuildEditorAsync(new UserGroup(){ParentGroupId = userGroup.Id}, updater: this, isNew: false);
            }
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

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageUserGroups))
            {
                return Unauthorized();
            }

            var userGroup = await _userGroupService.FindByIdAsync(id) as UserGroup;
            if (userGroup == null)
            {
                return NotFound();
            }

            var shape = await _userGroupsDisplayManager.BuildEditorAsync(userGroup, updater: this, isNew: false);

            return View(shape);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageUserGroups))
            {
                return Unauthorized();
            }

            var userGroup = await _userGroupService.FindByIdAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            var shape = await _userGroupsDisplayManager.UpdateEditorAsync(userGroup, updater: this, isNew: false);

            if (!ModelState.IsValid)
            {
                return View(shape);
            }

            var result = await _userGroupService.UpdateUserGroupAsync(userGroup);

            if (!ModelState.IsValid)
            {
                return View(shape);
            }
            _notifier.Success(TH["User group updated successfully"]);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageUserGroups))
            {
                return Unauthorized();
            }

            var userGroup = await _userGroupService.FindByIdAsync(id) as UserGroup;

            if (userGroup == null)
            {
                return NotFound();
            }

            await _userGroupService.DeleteUserGroupAsync(userGroup);

           
            _notifier.Success(TH["User group deleted successfully"]);        

            return RedirectToAction(nameof(Index));
        }
    }
}
