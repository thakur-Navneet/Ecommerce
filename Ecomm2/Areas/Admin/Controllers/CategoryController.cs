using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;
using Ecomm2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin + "," + SD.Role_Employee)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region APIs

        // API to retrieve all categories and return them as JSON.
        [HttpGet]
        public IActionResult GetAll()
        {
            var categoryList = _unitOfWork.Category.GetAll();  // Retrieving all categories.
            return Json(new { data = categoryList });  // Returning category list as JSON.
        }

        // API to delete a category by ID and return a success or failure message.
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryInDb = _unitOfWork.Category.Get(id);  // Retrieving the category by ID.
            if (categoryInDb == null)
                return Json(new { success = false, message = "Something went wrong !!!" });

            _unitOfWork.Category.Remove(categoryInDb);  // Removing the category.
            _unitOfWork.Save();  // Committing changes.
            return Json(new { success = true, message = "Data Delete Successfully !!!" });
        }

        #endregion

        // GET method for Upsert (Insert or Update).
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();  // Create a new empty Category object.
            if (id == null) return View(category);  // If id is null, return a new Category view (create mode).

            category = _unitOfWork.Category.Get(id.GetValueOrDefault());  // Retrieve the category by id for editing.
            if (category == null) return NotFound();  // If category is not found, return NotFound().
            return View(category);  // Return the category for editing.
        }

        // POST method for Upsert (Insert or Update).
        [HttpPost]
        [ValidateAntiForgeryToken]  // Protects against Cross-Site Request Forgery (CSRF) attacks.
        public IActionResult Upsert(Category category)
        {
            if (category == null) return NotFound();  // If category object is null, return NotFound().
            if (!ModelState.IsValid) return View(category);  // If validation fails, return the view with the same category object.

            if (category.Id == 0)
                _unitOfWork.Category.Add(category);  // If category ID is 0, add a new category.
            else
                _unitOfWork.Category.Update(category);  // Otherwise, update the existing category.

            _unitOfWork.Save();  // Save changes to the database.
            return RedirectToAction("Index");  // Redirect to the Index action.
        }
    }
}
