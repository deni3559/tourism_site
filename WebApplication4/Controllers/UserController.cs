using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using WebPortal.Controllers.CustomAuthorizeAttributes;
using WebPortal.DbStuff.Models;
using WebPortal.DbStuff.Repositories;
using WebPortal.DbStuff.Repositories.Interfaces;
using WebPortal.Enum;
using WebPortal.Hubs;
using WebPortal.Models.Users;
using WebPortal.Services;

namespace WebPortal.Controllers
{
    public class UserController : Controller
    {
        private IUserRepositrory _userRepositrory;
        private readonly IAuthService _authService;


        public UserController(
            IUserRepositrory userRepositrory,
            IAuthService authService)
        {
            _userRepositrory = userRepositrory;
            _authService = authService;
        }

        public IActionResult Index()
        {
            var usersViewModels = _userRepositrory
                .GetAll()
                .Select(x => new UserViewModel
                {
                    UserName = x.UserName,
                    Role = x.Role
                })
                .ToList();

            return View(usersViewModels);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(UserViewModel userViewModel)
        {
            var userDb = new User
            {
                UserName = userViewModel.UserName,
                Password = userViewModel.Password,
                Money = userViewModel.Money,
            };
            _userRepositrory.Add(userDb);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var viewModel = new ProfileViewModel();

            viewModel.Name = _authService.GetName();
            viewModel.Languages = System
                .Enum
                .GetValues<Language>()
                .ToList();
            viewModel.Language = _authService.GetLanguage();
            var userId = _authService.GetId();
            viewModel.Role = _authService.GetRole();

            return View(viewModel);
        }

        [Authorize]
        public IActionResult ChangeLanguage(Language lang)
        {
            var user = _authService.GetUser();
            user.Language = lang;
            _userRepositrory.Update(user);
            return RedirectToAction("Index", "Tourism");
        }

        [Authorize]
        [Role(Role.Admin)]
        [HttpGet]
        public IActionResult SetRoleUser()
        {
            var setRoleUsersViewModel = FillSetRoleUsersViewModel();

            return View(setRoleUsersViewModel);
        }

        private SetRoleUsersViewModel FillSetRoleUsersViewModel()
        {
            var meUser = _authService.GetUser();
            var allUser = _userRepositrory.GetAll().Where(user => user != meUser);

            var allUserVM = allUser.Select(user => new SelectListItem
            {
                Text = user.UserName,
                Value = user.Id.ToString(),
            }).ToList();

            var allRoles = System.Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString()
                }).ToList();


            var setRoleUsersViewModel = new SetRoleUsersViewModel
            {
                AllRoles = allRoles,
                AllUsers = allUserVM,
            };

            return setRoleUsersViewModel;
        }

        [Authorize]
        [Role(Role.Admin)]
        [HttpPost]
        public IActionResult SetRoleUser(int userId, int roleId)
        {
            if (!ModelState.IsValid)
            {
                var setRoleUsersViewModel = FillSetRoleUsersViewModel();
                return View(setRoleUsersViewModel);
            }

            var user = _userRepositrory.GetFirstById(userId);

            if(user == null)
            {
                throw new ArgumentNullException("User can't be null", nameof(user));
            }

            user.Role = (Role)roleId;

            _userRepositrory.Update(user);

            return RedirectToAction("Index");
        }
    }
}
