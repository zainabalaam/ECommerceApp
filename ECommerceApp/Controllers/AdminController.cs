using ECommerceApp.Models;
using ECommerceApp.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: /Admin/ManageRoles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // GET: /Admin/CreateRole
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }

        // POST: /Admin/CreateRole
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ManageRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Role already exists.");
                }
            }
            return View();
        }

        // GET: /Admin/AssignRole
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            // Create a view model to pass both user and roles
            var model = new AssignRoleViewModel
            {
                User = user,
                AvailableRoles = roles.Select(r => r.Name).ToList(),
                CurrentRoles = userRoles.ToList()
            };

            return View(model);
        }

        // POST: /Admin/AssignRole
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (result.Succeeded)
            {
                if (roles != null)
                {
                    result = await _userManager.AddToRolesAsync(user, roles);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ManageUsers");
                    }
                }
                else
                {
                    return RedirectToAction("ManageUsers");
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            var allRoles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new MultiSelectList(allRoles, "Name", "Name", roles);
            ViewBag.UserId = userId;
            return View(new AssignRoleViewModel { User = user, AvailableRoles = allRoles.Select(r => r.Name).ToList(), CurrentRoles = roles ?? new List<string>() });
        }


        // GET: /Admin/ManageUsers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<(ApplicationUser User, List<string> Roles)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add((user, roles.ToList()));
            }

            return View(usersWithRoles);
        }
    }
}