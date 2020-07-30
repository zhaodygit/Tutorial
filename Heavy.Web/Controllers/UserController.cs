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
                Email = userAddViewModel.Emial,
                IdCardNo = userAddViewModel.IdCardNo,
                BirthDate = userAddViewModel.BirthDate
            };
            var result = await _userManger.CreateAsync(user, userAddViewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManger.FindByIdAsync(id);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "用户找不到");
            }
            var updateUser = new UserUpdateViewModel
            {
                Id = user.Id,
                Emial = user.Email,
                UserName = user.UserName,
                IdCardNo = user.IdCardNo,
                BirthDate = user.BirthDate
            };
            return View(updateUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserUpdateViewModel userUpdateViewModel)
        {
            var user = await _userManger.FindByIdAsync(userUpdateViewModel.Id);
            if (user != null)
            {
                user.Email = userUpdateViewModel.Emial;
                user.BirthDate = userUpdateViewModel.BirthDate;
                user.IdCardNo = userUpdateViewModel.IdCardNo;

                var result = await _userManger.UpdateAsync(user);
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
            return RedirectToAction("Index");
        }
    }
}