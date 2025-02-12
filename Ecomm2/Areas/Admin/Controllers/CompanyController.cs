using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;
using Ecomm2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var companyInDB = _unitOfWork.Company.Get(Id);
            if (companyInDB == null)
                return Json(new { success = false, message = "Something went wrong" });
            _unitOfWork.Company.Remove(companyInDB);
            _unitOfWork.Save();
            return Json(new { success = true, message = "data deleted successfully!!!" });
        }
        #endregion
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null) return View(company);
            company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null) return BadRequest();
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (company == null) return NotFound();
            if (!ModelState.IsValid) return View(company);

            if (company.Id == 0)
                _unitOfWork.Company.Add(company);
            else
                _unitOfWork.Company.Update(company);

            _unitOfWork.Save();  
            return RedirectToAction("Index");
        }
    }
}