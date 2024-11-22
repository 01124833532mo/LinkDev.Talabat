using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Dashboard.Controllers
{
    public class UserController(RoleManager<IdentityRole> _roleManager,UserManager<ApplicationUser> _userManager) : Controller
    {
        public async Task <IActionResult> Index()
        {
			var users = await _userManager.Users
		.Select(u => new UserViewModel
		{
			Id = u.Id,
			DisplayName = u.DisplayName,
			UserName = u.UserName,
			Email=u.Email,
			PhoneNumber = u.PhoneNumber
		})
		.ToListAsync();

			foreach (var user in users)
			{
				// Await the GetRolesAsync call properly here
				user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
			}

			return View(users);
		}

		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var allRoles = await _roleManager.Roles.ToListAsync();
			var viewModel = new UserRoleViewModel()
			{
				UserId = user.Id,
				UserName = user.UserName,
				Roles = allRoles.Select(
					r => new RoleViewModel()
					{
						Id = r.Id,
						Name = r.Name,
						IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
					}).ToList()
			};

			return View(viewModel);
		}

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }

                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
