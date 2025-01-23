using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsPro.Models;
using System.Collections.Generic;
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
            List<SelectListItem> countryList = new List<SelectListItem> { };
            foreach (Country c in countries)
            {
                countryList.Add(new SelectListItem {Value = c.CountryID, Text = c.Name});
            }
            ViewBag.Countries = countryList;
            ViewBag.Mode = "Add";
            return View("Edit");
        }

        [HttpPost]
        public IActionResult Add(Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return RedirectToAction("List");
            } else {
                var countries = context.Countries.ToList();
                List<SelectListItem> countryList = new List<SelectListItem> { };
                foreach (Country c in countries)
                {
                    countryList.Add(new SelectListItem { Value = c.CountryID, Text = c.Name });
                }
                ViewBag.Countries = countryList;
                ViewBag.Mode = "Add";
                return View("Edit", customer);
            }
        }
        [HttpGet] //Load page to Edit an existing Entry
        public IActionResult Edit(int ID)
        {
            var customer = context.Customers.Find(ID);
            var countries = context.Countries.ToList();
            List<SelectListItem> countryList = new List<SelectListItem> { };
            foreach (Country c in countries)
            {
                countryList.Add(new SelectListItem { Value = c.CountryID, Text = c.Name });
            }
            ViewBag.Countries = countryList;
            ViewBag.Mode = "Edit";
            return View(customer); 
        }

        [HttpPost] //Edit an existing Entry
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Customers.Update(customer);
                context.SaveChanges();
                return RedirectToAction("List");
            } else {
                var countries = context.Countries.ToList();
                List<SelectListItem> countryList = new List<SelectListItem> { };
                foreach (Country c in countries)
                {
                    countryList.Add(new SelectListItem { Value = c.CountryID, Text = c.Name });
                }
                ViewBag.Countries = countryList;
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
