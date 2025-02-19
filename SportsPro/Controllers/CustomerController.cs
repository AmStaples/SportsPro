using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Linq;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {

        private SportsProContext context { get; set; }

        public CustomerController(SportsProContext ctx)
        {
            context = ctx;
        }

        public JsonResult CheckEmail(string email, string customerId)
        {
            var id = 0;
            int.TryParse(customerId, out id);

            bool emailExists = context.Customers.Any(
                c => c.Email == email && c.CustomerID != id);
            
            if (emailExists)
            {
                return Json("Email address already in use.");
            }
            else
            {
                return Json(true);
            }
        }

        [NonAction]
        private void ValidateEmail(Customer customer)
        {
            var duplicateExists = context.Customers.Any(c =>
                c.Email == customer.Email && c.CustomerID != customer.CustomerID);

            if (duplicateExists)
            {
                ModelState.AddModelError(
                    nameof(Customer.Email), "Email address already in use.");
            }
        }

        [Route("[controller]s")]
        [HttpGet]
        public IActionResult List()
        {
            var customers = context.Customers.ToList();
            return View(customers); 
        }

        [HttpGet] 
        public IActionResult Add()
        {
            var countries = context.Countries.ToList();
            ViewBag.Countries = countries;
            ViewBag.Mode = "Add";
            return View("Edit");
        }

        [HttpPost]
        public IActionResult Add(Customer customer)
        {
            ValidateEmail(customer);

            if (ModelState.IsValid)
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return RedirectToAction("List");
            } else {
                var countries = context.Countries.ToList();
                ViewBag.Countries = countries;
                ViewBag.Mode = "Add";
                return View("Edit", customer);
            }
        }
        [HttpGet] //Load page to Edit an existing Entry
        public IActionResult Edit(int ID)
        {
            var customer = context.Customers.Find(ID);
            var countries = context.Countries.ToList();
            ViewBag.Countries = countries;
            ViewBag.Mode = "Edit";
            return View(customer); 
        }

        [HttpPost] //Edit an existing Entry
        public IActionResult Edit(Customer customer)
        {
            ValidateEmail(customer);

            if (ModelState.IsValid)
            {
                context.Customers.Update(customer);
                context.SaveChanges();
                return RedirectToAction("List");
            } else {
                var countries = context.Countries.ToList();
                ViewBag.Countries = countries;
                ViewBag.Mode = "Edit";
                return View(customer);
            }
        }

        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            customer = context.Customers.Find(customer.CustomerID);
            if (customer == null) { return NotFound(); }
            context.Customers.Remove(customer);
            context.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public  IActionResult Delete(int id)
        {
            var customer = context.Customers.Find(id);
            if (customer == null) { return NotFound(); }
            return View(customer);
        }
    }
}
