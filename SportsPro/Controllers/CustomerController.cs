using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;
using System.Linq;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {

        private Repository<Customer> customers { get; set; }
        private Repository<Country> countries { get; set; }

        public CustomerController(Repository<Customer> customers, Repository<Country> countries)
        {
            this.customers = customers;
            this.countries = countries;
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
            var customers = this.customers.List(new QueryOptions<Customer>());
            return View(customers); 
        }

        [HttpGet] 
        public IActionResult Add()
        {
            var countries = this.countries.List(new QueryOptions<Country>());
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
                customers.Insert(customer);
                customers.Save();
                return RedirectToAction("List");
            } else {
                var countries = this.countries.List(new QueryOptions<Country>());
                ViewBag.Countries = countries;
                ViewBag.Mode = "Add";
                return View("Edit", customer);
            }
        }

        [HttpGet] //Load page to Edit an existing Entry
        public IActionResult Edit(int ID)
        {
            var customer = this.customers.Get(ID);
            var countries = this.countries.List(new QueryOptions<Country>());
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
                customers.Update(customer);
                customers.Save();
                return RedirectToAction("List");
            } else {
                var countries = this.countries.List(new QueryOptions<Country>());
                ViewBag.Countries = countries;
                ViewBag.Mode = "Edit";
                return View(customer);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = customers.Get(id);
            if (customer == null) { return NotFound(); }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            customer = customers.Get((int) customer.CustomerID);
            if (customer == null) return NotFound();
            customers.Delete(customer);
            customers.Save();
            return RedirectToAction("List");
        }
    }
}
