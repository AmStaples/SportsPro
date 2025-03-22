using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using SportsPro.Models.DataLayer;
using System.Collections.Generic;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private Repository<Incident> incidents { get; set; }
        private Repository<Customer> customers { get; set; }
        private Repository<Product> products { get; set; }
        private Repository<Technician> technicians { get; set; }

        public IncidentController(Repository<Incident> incidents, Repository<Customer> customers,
            Repository<Product> products, Repository<Technician> technicians)
        {
            this.incidents = incidents;
            this.customers = customers;
            this.products = products;
            this.technicians = technicians;
        }

        [Route("[controller]s")]
        [HttpGet]
        public ViewResult List(string filter)
        {
            var queryOptions = new QueryOptions<Incident>();
            queryOptions.Includes = "Customer, Product";
            queryOptions.OrderBy = i => i.IncidentID;

            if (filter == "Unassigned")
            {
                queryOptions.Where = i => i.TechnicianID == null;
            }
            else if (filter == "Open")
            {
                queryOptions.Where = i => i.DateClosed == null;
            }

            var incidents = (List<Incident>) this.incidents.List(queryOptions);

            var viewModel = new IncidentListViewModel
            {
                Incidents = incidents,
                Filter = filter
            };

            return View(viewModel);
        }


        [HttpGet]
        public ViewResult Add()
        {
            var incident = new Incident();
            var viewModel = new IncidentEditViewModel
            {
                Incident = incident,
                Mode = "Add",
                Customers = (List<Customer>) customers.List(new QueryOptions<Customer>()),
                Products = (List<Product>) products.List(new QueryOptions<Product>()),
                Technicians = (List<Technician>) technicians.List(new QueryOptions<Technician>())
            };

            return View("Edit", viewModel);
        }


        [HttpPost]
        public ActionResult Add(Incident incident)
        {
            if (ModelState.IsValid)
            {
                incidents.Insert(incident);
                incidents.Save();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Mode = "Add";
                PopulateDropdowns();
                return View("Edit", incident);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var incident = incidents.Get(id);
            if (incident == null)
            {
                return NotFound();
            }
            PopulateDropdowns();
            var viewModel = new IncidentEditViewModel
            {
                Incident = incident,
                Customers = (List<Customer>) customers.List(new QueryOptions<Customer>()),
                Products = (List<Product>) products.List(new QueryOptions<Product>()),
                Technicians = (List<Technician>) technicians.List(new QueryOptions<Technician>()),
                Mode = "Edit"

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Incident incident)
        {
            if (ModelState.IsValid)
            {
                incidents.Update(incident);
                incidents.Save();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Mode = "Edit";
                PopulateDropdowns();
                return View(incident);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var incident = incidents.Get(id);
            if (incident == null)
            {
                return NotFound();
            }
            return View(incident);
        }

        [HttpPost]
        public ActionResult Delete(Incident incident)
        {
            incident = incidents.Get(incident.IncidentID);
            if (incident == null)
            {
                return NotFound();
            }
            incidents.Delete(incident);
            incidents.Save();
            return RedirectToAction("List");
        }

        private void PopulateDropdowns()
        {
            ViewBag.Customers = GetCustomers();
            ViewBag.Products = GetProducts();
            ViewBag.Technicians = GetTechnicians();
        }

        private SelectList GetCustomers()
        {
            var queryOptions = new QueryOptions<Customer>();
            queryOptions.OrderBy = c => c.FirstName + " " + c.LastName;

            var customers = this.customers.List(queryOptions);

            return new SelectList(customers, "CustomerID", "FullName");
        }

        private SelectList GetProducts()
        {
            var queryOptions = new QueryOptions<Product>();
            queryOptions.OrderBy = p => p.Name;

            var products = this.products.List(queryOptions);

            return new SelectList(products, "ProductID", "Name");
        }

        private SelectList GetTechnicians()
        {
            var queryOptions = new QueryOptions<Technician>();
            queryOptions.OrderBy = t => t.Name;

            var technicians = this.technicians.List(queryOptions);

            return new SelectList(technicians, "TechnicianID", "Name");
        }
    }
}