using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models.ViewModels;
using Ecomm2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ecomm2.Utility;
using Microsoft.AspNetCore.Http;

namespace Ecomm2.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null)
            {
                ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
                {
                    ListCart = new List<ShoppingCart>()
                };
                return View(shoppingCartVM);
            }
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll
                (sc => sc.ApplicationUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader() { }
            };
            //*****
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(au => au.Id == claims.Value);
            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedonQunatity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += list.Count * list.Price;
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "......";
                }
            }
            return View(ShoppingCartVM);
        }
        public IActionResult plus(int id)
        {
            var cart = _unitOfWork.ShoppingCart.Get(id);
            if (cart == null) return NotFound();
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int id)
        {
            var cart = _unitOfWork.ShoppingCart.Get(id);
            if (cart == null) return NotFound();
            if (cart.Count == 1)
                cart.Count = 1;
            else
                cart.Count -= 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var cart = _unitOfWork.ShoppingCart.Get(id);
            if (cart == null) return NotFound();
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            // session
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null) return NotFound();
            var count = _unitOfWork.ShoppingCart.GetAll(
                sc => sc.ApplicationUserId == claims.Value).ToList().Count;
            HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null) return NotFound();
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll
                    (sc => sc.ApplicationUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(au => au.Id == claims.Value);
            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedonQunatity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += list.Count * list.Price;
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "......";
                }
            }
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null) return NotFound();
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(au => au.Id == claims.Value);
            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll
                (sc => sc.ApplicationUserId == claims.Value, includeProperties: "Product");
            ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claims.Value;
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedonQunatity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = list.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = list.Price,
                    Count = list.Count
                };
                ShoppingCartVM.OrderHeader.OrderTotal += list.Price * list.Count;
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWork.Save();
            //Session Count
            HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, 0);
            return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
        }
        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}
