using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;
using Ecomm2.Models.ViewModels;
using Ecomm2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecomm2.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll
                    (sc=>sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount,count);
            }
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(productList);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll
                    (sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }
            //*******
            var productInDB = _unitOfWork.Product.FirstOrDefault(p => p.Id == id, includeProperties: "Category,CoverType");
            if (productInDB == null) return NotFound();
            var shoppingCart = new ShoppingCart()
            {
                Product = productInDB,
                ProductId = id
            };
            return View(shoppingCart);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if(claims == null) return NotFound();
                shoppingCart.ApplicationUserId= claims.Value;
                var shoppingCartInDB = _unitOfWork.ShoppingCart.FirstOrDefault
                    (sc => sc.ApplicationUserId == claims.Value && sc.ProductId == shoppingCart.ProductId);


                if (shoppingCartInDB == null)
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                else
                    shoppingCartInDB.Count += shoppingCart.Count;
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productInDB = _unitOfWork.Product.FirstOrDefault(p => p.Id == shoppingCart.ProductId,
                    includeProperties: "Category,CoverType");
                if (productInDB == null) return NotFound();
                 shoppingCart = new ShoppingCart()
                {
                    Product = productInDB,
                    ProductId = shoppingCart.ProductId
                };
                return View(shoppingCart);
            }
            
        }
        
    }
}