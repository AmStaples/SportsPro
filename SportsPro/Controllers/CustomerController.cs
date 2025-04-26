using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {

        private IRepository<Customer> customers { get; set; }
        private IRepository<Country> countries { get; set; }

        public CustomerController(IRepository<Customer> cuRep, IRepository<Country> coRep)
        {
            customers = cuRep;
            countries = coRep;
        }

        [NonAction]
        private void ValidateEmail(Customer customer)
        {
            string error = Check.EmailExists(customers, customer.Email, customer.CustomerID ?? 0);

            if (!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError(nameof(Customer.Email), error);
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
            ViewBag.Countries = countries.List(new QueryOptions<Country>());
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
                TempData["message"] = $"{customer.FullName} was added.";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Countries = countries.List(new QueryOptions<Country>());
                ViewBag.Mode = "Add";
                return View("Edit", customer);
            }
        }

        [HttpGet] //Load page to Edit an existing Entry
        public IActionResult Edit(int Id)
        {
            var customer = customers.Get(Id);
            if (customer == null) return NotFound();
            ViewBag.Countries = countries.List(new QueryOptions<Country>());
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
                TempData["message"] = $"{customer.FullName} was edited.";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Countries = countries.List(new QueryOptions<Country>());
                ViewBag.Mode = "Edit";
                return View(customer);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = customers.Get(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Delete(ConfirmDeletionViewModel model)
        {
            var customer = customers.Get(model.Id);
            if (customer == null) return NotFound();
            customers.Delete(customer);
            customers.Save();
            TempData["message"] = $"{customer.FullName} was deleted.";
            return RedirectToAction("List");
        }
    }
}
