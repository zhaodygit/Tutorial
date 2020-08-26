using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Data;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
    //[Authorize(Roles = "Administrators")]
    [Authorize(Policy = "编辑")]
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

        /// <summary>
        /// 再用户管理中,如果用户名称和邮箱不一致将无法登录 原因AspNetUsers表中NormalizedEmail字段值是用户名的大写将无法登录,改未邮箱大写就可以登录
        /// </summary>
        /// <param name="userAddViewModel"></param>
        /// <returns></returns>
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
                NormalizedUserName = userAddViewModel.Emial.ToUpper(),
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
            var claims = await _userManger.GetClaimsAsync(user);
            var updateUser = new UserUpdateViewModel
            {
                Id = user.Id,
                Emial = user.Email,
                UserName = user.UserName,
                IdCardNo = user.IdCardNo,
                BirthDate = user.BirthDate,
                Claims = claims.Select(x => x.Value).ToList()
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
                user.NormalizedUserName = userUpdateViewModel.Emial.ToUpper();
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

        public async Task<IActionResult> ManageClaims(string id)
        {
            var user = await _userManger.FindByIdAsync(id);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "用户找不到");
                return RedirectToAction("Index");
            }
            var vm = new ManageClaimsViewModel
            {
                UserId = id,
                AllCliams = ClaimTypes.AllClaimTypeList
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ManageClaims(ManageClaimsViewModel vm)
        {
            var user = await _userManger.FindByIdAsync(vm.UserId);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var claim = new IdentityUserClaim<string>
            {
                ClaimType = vm.ClaimId,
                ClaimValue = vm.ClaimId
            };
            user.Claims.Add(claim);

            var result = await _userManger.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("EditUser", new { id = vm.UserId });
            }

            ModelState.AddModelError(string.Empty, "编辑用户Claims时发生错误");
            return View(vm);
        }
    }
}