﻿using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsPro.Models.DataLayer;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace SportsPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IncidentController : Controller
    {
        private IRepository<Incident> incidents { get; set; }
        private IRepository<Customer> customers { get; set; }
        private IRepository<Product> products { get; set; }
        private IRepository<Technician> technicians { get; set; }

        public IncidentController(IRepository<Incident> iRep, IRepository<Customer> cRep, IRepository<Product> pRep, IRepository<Technician> tRep)
        {
            incidents = iRep;
            customers = cRep;
            products = pRep;
            technicians = tRep;
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

            var incidents = (List<Incident>)this.incidents.List(queryOptions);

            var viewModel = new IncidentListViewModel
            {
                Incidents = incidents,
                Filter = filter
            };
            ViewBag.Filter = filter;
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
                Customers = (List<Customer>)customers.List(new QueryOptions<Customer>()),
                Products = (List<Product>)products.List(new QueryOptions<Product>()),
                Technicians = (List<Technician>)technicians.List(new QueryOptions<Technician>())
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
                TempData["message"] = $"{incident.Title} was added.";
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
                Customers = (List<Customer>)customers.List(new QueryOptions<Customer>()),
                Products = (List<Product>)products.List(new QueryOptions<Product>()),
                Technicians = (List<Technician>)technicians.List(new QueryOptions<Technician>()),
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
                TempData["message"] = $"{incident.Title} was edited.";
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
        public ActionResult Delete(ConfirmDeletionViewModel model)
        {
            var incident = incidents.Get(model.Id);
            if (incident == null)
            {
                return NotFound();
            }
            incidents.Delete(incident);
            incidents.Save();
            TempData["message"] = $"{model.Name} was deleted.";
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