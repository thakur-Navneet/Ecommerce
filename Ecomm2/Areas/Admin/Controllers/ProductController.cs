using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models.ViewModels;
using Ecomm2.Models;
using Microsoft.AspNetCore.Authorization;
using Ecomm2.Utility;

namespace Ecomm2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofwork,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var ProductList = _unitofwork.Product.GetAll();
            return Json(new { data = ProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productindb = _unitofwork.Product.Get(id);
            if (productindb == null)
                return Json(new { success = false, message = "unable to delete data !!!" });
            //Image Delete
            var webRootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, productindb.ImageURL.Trim('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitofwork.Product.Remove(productindb);
            _unitofwork.Save();
            return Json(new { success = true, message = "data deleted successfully !!!!" });
        }
        #endregion
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitofwork.Category.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                }),
                CoverTypeList = _unitofwork.CoverType.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                })

            };
            if (id == null) return View(productVM);
            productVM.Product = _unitofwork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null) return NotFound();
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    var uploads = Path.Combine(webRootPath, @"Images\Productss");
                    if (productVM.Product.Id != 0)
                    {
                        var imageExists = _unitofwork.Product.Get(productVM.Product.Id).ImageURL;
                        productVM.Product.ImageURL = imageExists;
                    }
                    if (productVM.Product.ImageURL != null)
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageURL.Trim('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.ImageURL = @"\Images\Productss\" + fileName + extension;
                }
                else
                {
                    if (productVM.Product.Id != 0)
                    {
                        var imagePath = _unitofwork.Product.Get(productVM.Product.Id).ImageURL;
                        productVM.Product.ImageURL = imagePath;
                    }
                }
                if (productVM.Product.Id == 0)
                    _unitofwork.Product.Add(productVM.Product);
                else
                    _unitofwork.Product.Update(productVM.Product);
                _unitofwork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM = new ProductVM()
                {
                    Product = new Product(),
                    CategoryList = _unitofwork.Category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()
                    }),
                    CoverTypeList = _unitofwork.CoverType.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()
                    })

                };
                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _unitofwork.Product.Get(productVM.Product.Id);
                }
                return View(productVM);
            }
        }
    }
}