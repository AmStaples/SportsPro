using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;
using System.Linq;

namespace SportsPro.Controllers
{
    [Authorize]
    public class TechIncidentController : Controller
    {
        private const string Tech_ID = "techID";

        private readonly IRepository<Technician> _technicianRepo;
        private readonly IRepository<Incident> _incidentRepo;
        private readonly ISession _session;

        public TechIncidentController(
            IRepository<Technician> technicianRepo,
            IRepository<Incident> incidentRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _technicianRepo = technicianRepo;
            _incidentRepo = incidentRepo;
            _session = httpContextAccessor.HttpContext.Session; 
        }

        [HttpGet]
        public ViewResult Index()
        {
            var queryOptions = new QueryOptions<Technician>
            {
                Where = t => t.TechnicianID > -1, 
                OrderBy = c => c.Name
            };

            ViewBag.Technicians = _technicianRepo.List(queryOptions);

            var technician = new Technician();
            int? techID = _session.GetInt32(Tech_ID);
            if (techID.HasValue)
            {
                technician = _technicianRepo.Get(techID.Value);
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
            else
            {
                _session.SetInt32(Tech_ID, technician.TechnicianID);
                return RedirectToAction("List", new { id = technician.TechnicianID });
            }
        }

        [HttpGet]
        public IActionResult List(int id)
        {
            var technician = _technicianRepo.Get(id);
            if (technician == null)
            {
                TempData["message"] = "Please select a technician!";
                return RedirectToAction("Index");
            }

            var queryOptions = new QueryOptions<Incident>
            {
                Includes = "Customer, Product",
                Where = i => i.TechnicianID == id && i.DateClosed == null,
                OrderBy = i => i.DateOpened
            };

            var incidents = _incidentRepo.List(queryOptions).ToList();

            var model = new TechIncidentViewModel
            {
                Technician = technician,
                Incidents = incidents
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            int? techID = _session.GetInt32(Tech_ID);
            if (!techID.HasValue)
            {
                TempData["message"] = "Please select a technician!";
                return RedirectToAction("Index");
            }

            var technician = _technicianRepo.Get(techID.Value);
            if (technician == null)
            {
                return NotFound();
            }

            var queryOptions = new QueryOptions<Incident>
            {
                Includes = "Customer, Product",
                Where = i => i.IncidentID == id
            };

            var incident = _incidentRepo.List(queryOptions).FirstOrDefault();
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

            var incident = _incidentRepo.Get(model.Incident.IncidentID);
            if (incident == null)
            {
                return NotFound();
            }

            incident.Description = model.Incident.Description;
            incident.DateClosed = model.Incident.DateClosed;

            _incidentRepo.Update(incident);
            _incidentRepo.Save();

            int? techID = _session.GetInt32(Tech_ID);
            return RedirectToAction("List", new { id = techID });
        }
    }
}