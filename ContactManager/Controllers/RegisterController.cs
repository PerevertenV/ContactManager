using CM.Data.Repository.IRepository;
using CM.Models.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text.RegularExpressions;
using CM.Data.Service.IService;

namespace ContactManager.Controllers
{
	public class RegisterController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IServices _services;
		public RegisterController(IUnitOfWork unitOfWork, IServices services)
		{
			_unitOfWork = unitOfWork;
			_services = services;
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Index(User obj, IFormCollection form)
		{
			string confirmPassword = form["confirmPassword"];
			List<User> users = _unitOfWork.User.GetAll().ToList();
			bool PasswordChecker = Regex.IsMatch(obj.Password, "[a-zA-Z]");
			foreach (User user in users)
			{
				if (obj.Login == user.Login)
				{
					ModelState.AddModelError("login", "User with this login already exists");
					return View();
				}
			}
			if (obj.Password.Length < 6 || obj.Password.Length > 15)
			{
				ModelState.AddModelError("password", "Password must contain from 5 to 15 elements");
				return View();
			}
			else if (!PasswordChecker)
			{
				ModelState.AddModelError("password", "Password must contain at least one letter");
				return View();
			}
			else if (confirmPassword != obj.Password)
			{
				ModelState.AddModelError("password", "Passwords must match ");
				return View();
			}
			else
			{
				var UserToAdding = new User
				{
					Login = obj.Login,
					Password = _services.User.PasswordHashCoder(obj.Password),
				};

				_unitOfWork.User.Add(UserToAdding);

				var UserId = _unitOfWork.User.GetFirstOrDefault(u => u.Login == obj.Login).ID;
				if (!User.Identity.IsAuthenticated)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, obj.Login),
						new Claim("UserID", UserId.ToString())
					};

					var claimsIdentity = new ClaimsIdentity(
						claims, CookieAuthenticationDefaults.AuthenticationScheme);

					var authProperties = new AuthenticationProperties
					{
						IsPersistent = true
					};

					HttpContext.SignInAsync(
					   CookieAuthenticationDefaults.AuthenticationScheme,
					   new ClaimsPrincipal(claimsIdentity),
					   authProperties).GetAwaiter().GetResult();
				}
				TempData["success"] = "The account has been created successfully! 😀";
				return Redirect("Home/Index");
			}
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
