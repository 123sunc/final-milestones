using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKYResturant.Models;
using System.Diagnostics;

namespace SKYResturant.Controllers
{
    
    public class HomeController : Controller
    {
        private   ILogger<HomeController> _logger;
        private readonly SkyDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, SkyDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;

            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin"); // Redirect to the admin/index page
            }
            else
            {
                return _context.Menus != null ?
                              View(await _context.Menus.ToListAsync()) :
                              Problem("Entity set 'SkyDbContext.Menus'  is null.");
            }  //return View();
        }
        public async Task<IActionResult> AddToCart(int ItemID)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Handle the case where the user is not authenticated
                return RedirectToAction("Login", "Account");
            }

            var customer = await _context.CheckoutCustomer.FirstOrDefaultAsync(c => c.Email == user.Email);
            if (customer == null)
            {
                // Handle the case where the customer is not found
                throw new Exception($"CheckoutCustomer not found for user '{user.Email}'.");
            }

            var item = await _context.BasketItem.FirstOrDefaultAsync(i => i.StockID == ItemID && i.BasketID == customer.BasketID);
            if (item == null)
            {
                BasketItem newItem = new BasketItem
                {
                    BasketID = customer.BasketID,
                    StockID = ItemID,
                    Quantity = 1,
                };
                _context.BasketItem.Add(newItem);
            }
            else
            {
                item.Quantity++;
                _context.Entry(item).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                // Handle concurrency exception
                throw new Exception($"Concurrency exception occurred while saving changes.", e);
            }

            // Redirect to a specific action method or page
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Aboutus()
        {
            return View();
        } 
        //public IActionResult Menu()
        //{
        //    return View("~/Views/Home/Menu.cshtml");
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}