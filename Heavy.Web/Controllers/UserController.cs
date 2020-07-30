using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;

        public UserController(UserManager<ApplicationUser> userManger)
        {
            this._userManger = userManger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManger.Users.ToListAsync();
            return View(user);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserAddViewModel userAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAddViewModel);
            }
            var user = new ApplicationUser
            {
                UserName = userAddViewModel.UserName,
                Email = userAddViewModel.Emial
            };
            var result = await _userManger.CreateAsync(user, userAddViewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", await _userManger.Users.ToArrayAsync());
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(userAddViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(String id)
        {
            var user = await _userManger.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManger.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "删除用户发生错误");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "用户找不到");
            }
            return View("Index", await _userManger.Users.ToListAsync());
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManger.FindByIdAsync(id);
            return View(user);
        }
    }
}