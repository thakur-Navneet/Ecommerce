using Ecomm2.DataAccess.Data;
using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;
using Ecomm2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _context.ApplicationUsers.ToList();
            var roles = _context.Roles.ToList();
            var userRoles = _context.UserRoles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                if (user.CompanyId == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
                if (user.CompanyId != null)
                {
                    user.Company = new Company()
                    {
                        Name = _unitOfWork.Company.Get(Convert.ToInt32(user.CompanyId)).Name 
                    };
                }
            }
            // Remove admin user
            var adminUser = userList.FirstOrDefault(u => u.Role == SD.Role_Admin);
            if(adminUser != null)
            {
                userList.Remove(adminUser);
            }
            return Json(new { data = userList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            bool isLocked = false;
            var userInDB = _context.ApplicationUsers.FirstOrDefault(au => au.Id == id);
            if (userInDB == null)
                return Json(new { success = false, message = "Something went wrong while lock and Unlock user !" });
            if(userInDB != null && userInDB.LockoutEnd>DateTime.Now)
            {
                userInDB.LockoutEnd = DateTime.Now;
                isLocked = false;
            }
            else
            {
                userInDB.LockoutEnd = DateTime.Now.AddYears(50);
                isLocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true,message= isLocked==true ? "User Successfullly Locked" : "User Unsuccessfully unlocked" });
            
        }
        #endregion
    }
}
