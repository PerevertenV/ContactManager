using CM.Data.Repository.IRepository;
using CM.Models.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CM.Data.Service.IService;

namespace ContactManager.Controllers
{
	public class LoginController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IServices _services;
		public LoginController(IUnitOfWork unitOfWork, IServices services)
		{
			_unitOfWork = unitOfWork;
			_services = services;
		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Index(User obj)
		{
			bool success = true;
			List<User> users = _unitOfWork.User.GetAll().ToList();
			foreach (var user in users)
			{
				if (user.Login == obj.Login)
				{
					string Decodet = _services.User.DecryptString(user.Password);
					success = false;
					if (obj.Password == Decodet)
					{
						var claims = new List<Claim>
						{
							new Claim(ClaimTypes.Name, user.Login),
							new Claim("UserID", user.ID.ToString())
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

						TempData["success"] = "You are login! 😀";
						return Redirect("Home/Index");
					}
					else
					{
						ModelState.AddModelError("password", "Incorrect password, try again.");
						return View();
					}
				}
			}
			if (success)
			{
				ModelState.AddModelError("login", "Ми не знайшли користувача із таким username");
				return View();
			}
			return View();
		}
	}
}
