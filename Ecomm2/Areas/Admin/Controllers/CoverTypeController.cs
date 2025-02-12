using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;
using Ecomm2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
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
            var coverTypeList = _unitOfWork.CoverType.GetAll();
            return Json(new { data = coverTypeList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var coverInDB = _unitOfWork.CoverType.Get(id);
            if (coverInDB == null)
                return Json(new { success = false, message = "Something went wrong !!!" });

            _unitOfWork.CoverType.Remove(coverInDB);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Delete Successfully !!!" });
        }
        #endregion

        // GET method for Upsert (Insert or Update).
        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null) return View(coverType);

            coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (coverType == null) return NotFound();
            return View(coverType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null) return NotFound();
            if (!ModelState.IsValid) return View(coverType);

            if (coverType.Id == 0)
                _unitOfWork.CoverType.Add(coverType);
            else
                _unitOfWork.CoverType.Update(coverType);

            _unitOfWork.Save(); 
            return RedirectToAction("Index");
        }
    }
}
