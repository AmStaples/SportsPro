using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ViewResult List(string filter)
        {
            IQueryable<Incident> incidentsQuery = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .OrderBy(i => i.IncidentID);

            if (filter == "Unassigned")
            {
                incidentsQuery = incidentsQuery.Where(i => i.TechnicianID == null);
            }
            else if (filter == "Open")
            {
                incidentsQuery = incidentsQuery.Where(i => i.DateClosed == null);
            }

            var incidents = incidentsQuery.ToList();

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
            ViewBag.Mode = "Add";
            PopulateDropdowns();
            return View("Edit", incident);
        }

        [HttpPost]
        public ActionResult Add(Incident incident)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Add(incident);
                context.SaveChanges();
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
            var incident = context.Incidents.Find(id);
            if (incident == null)
            {
                return NotFound();
            }
            ViewBag.Mode = "Edit";
            PopulateDropdowns();
            return View(incident);
        }

        [HttpPost]
        public ActionResult Edit(Incident incident)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Update(incident);
                context.SaveChanges();
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
            var incident = context.Incidents.Find(id);
            if (incident == null)
            {
                return NotFound();
            }
            return View(incident);
        }

        [HttpPost]
        public ActionResult Delete(Incident incident)
        {
            incident = context.Incidents.Find(incident.IncidentID);
            if (incident == null)
            {
                return NotFound();
            }
            context.Remove(incident);
            context.SaveChanges();
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
            var customers = context.Customers
                .Select(c => new
                {
                    c.CustomerID,
                    FullName = c.FirstName + " " + c.LastName
                })
                .OrderBy(c => c.FullName)
                .ToList();

            return new SelectList(customers, "CustomerID", "FullName");
        }

        private SelectList GetProducts()
        {
            var products = context.Products
                .OrderBy(p => p.Name)
                .ToList();

            return new SelectList(products, "ProductID", "Name");
        }

        private SelectList GetTechnicians()
        {
            var technicians = context.Technicians
                .OrderBy(t => t.Name)
                .ToList();

            return new SelectList(technicians, "TechnicianID", "Name");
        }
    }
}