using CM.Data.Repository.IRepository;
using CM.Data.Service.IService;
using CM.Models.Models;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ContactManager.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IServices _services; 

		public HomeController(ILogger<HomeController> logger, 
			IUnitOfWork unitOfWork,
			IServices services)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
			_services = services;
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Index(IFormFile file)
		{
			if(file == null || file.Length <= 0) 
			{
				TempData["error"] = "file empty";
				return RedirectToAction(nameof(Index));
			}
			ContactInfo? contactInfo = _services.ContactInfo.ConvertFromCsv(file);

            if (contactInfo == null) 
			{
				TempData["error"] = "Incorect data in file";
                return RedirectToAction(nameof(Index));
            }

			var user = HttpContext.User;
			var userId = user.FindFirstValue("UserID");
			contactInfo.UserId = int.Parse(userId);

			_unitOfWork.ContactInfo.Add(contactInfo);
            TempData["success"] = "Information added successfully";

            return RedirectToAction(nameof(Index));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var user = HttpContext.User;
            var userId = user.FindFirstValue("UserID");

            List<ContactInfo> ObjectsFromDb = _unitOfWork.ContactInfo
				.GetAll(u => u.UserId == int.Parse(userId)).ToList();

            return Json(new { data = ObjectsFromDb });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var InfoToBeDeleted = _unitOfWork.ContactInfo.GetFirstOrDefault(u => u.Id == id);
            if (InfoToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.ContactInfo.Delete(InfoToBeDeleted);

            return Json(new { succes = true, message = "Deleted successfully" });

        }
        #endregion
    }
}
