using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace SportsPro.Controllers
{
    public class TechIncidentController : Controller
    {
        private const string Tech_ID = "techID";

        private IRepository<Technician> technicians { get; set; }
        private IRepository<Incident> incidents { get; set; }
        private IHttpContextAccessor accessor { get; set; }

        public TechIncidentController(IRepository<Technician> tRep, IRepository<Incident> iRep, IHttpContextAccessor accessor)
        {
            technicians = tRep;
            incidents = iRep;
            this.accessor = accessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var queryOptions = new QueryOptions<Technician>();
            queryOptions.Where = t => t.TechnicianID > -1; // skipping unassigned
            queryOptions.OrderBy = c => c.Name;

            ViewBag.Technicians = technicians.List(queryOptions);

            var technician = new Technician();

            int? techID = accessor.HttpContext.Session.GetInt32(Tech_ID);
            if (techID != null) {
                technician = technicians.Get((int)techID);
            }

            return View(technician);
        }

        [HttpPost]
        public IActionResult List(Technician technician)
        {
            if (technician.TechnicianID == 0)
            {
                TempData["message"] = "Please select a technician!";
                return RedirectToAction("Index");
            }
            else {
                accessor.HttpContext.Session.SetInt32(Tech_ID, technician.TechnicianID);
                return RedirectToAction("List", new { id = technician.TechnicianID });
            }
        }

        [HttpGet]
        public IActionResult List(int id)
        {
            var technician = technicians.Get(id);
            if (technician == null)
            {
                TempData["message"] = "Please select a technician!";
                return RedirectToAction("Index");
            }
            else
            {
                var queryOptions = new QueryOptions<Incident>();
                queryOptions.Includes = "Customer, Product";
                queryOptions.Where = i => i.TechnicianID == id && i.DateClosed == null;
                queryOptions.OrderBy = i => i.DateOpened;

                var incidents = (List<Incident>)this.incidents.List(queryOptions);

                var model = new TechIncidentViewModel
                {
                    Technician = technician,
                    Incidents = incidents
                };
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id) {
            int? techID = accessor.HttpContext.Session.GetInt32(Tech_ID);
            if (techID == null)
            {
                TempData["message"] = "Please select a technician!";
                return RedirectToAction("Index");
            }

            var technician = technicians.Get((int)techID);
            if (technician == null)
            {
                return NotFound();
            }

            var queryOptions = new QueryOptions<Incident>();
            queryOptions.Includes = "Customer, Product";
            queryOptions.Where = i => i.IncidentID == id;

            var incident = incidents.List(queryOptions).FirstOrDefault();
            if (incident == null)
            {
                return NotFound();
            }

            var model = new TechIncidentViewModel
            {
                Technician = technician,
                Incident = incident
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(TechIncidentViewModel model)
        {
            if (model.Incident.IncidentID == 0)
            {
                return NotFound();
            }

            Incident incident = incidents.Get(model.Incident.IncidentID);
            if (incident == null)
            {
                return NotFound();
            }

            incident.Description = model.Incident.Description;
            incident.DateClosed = model.Incident.DateClosed;

            incidents.Update(incident);
            incidents.Save();

            int? techID = accessor.HttpContext.Session.GetInt32(Tech_ID);
            return RedirectToAction("List", new { id = techID });
        }
    }
}