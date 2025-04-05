using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private Repository<Technician> technicians { get; set; }
        private Repository<Incident> incidents { get; set; }

        public TechnicianController(SportsProContext context)
        {
            technicians = new Repository<Technician>(context);
            incidents = new Repository<Incident>(context);
        }

        [Route("[controller]s")]
        [HttpGet]
        public ViewResult List()
        {
            var queryOptions = new QueryOptions<Technician>();
            queryOptions.OrderBy = t => t.Name;

            var technicians = this.technicians.List(queryOptions);
            return View(technicians);
        }

        [HttpGet]
        public ViewResult Add()
        {
            var technician = new Technician();
            ViewBag.Mode = "Add";
            return View("Edit", technician);
        }

        [HttpPost]
        public ActionResult Add(Technician technician)
        {
            if (ModelState.IsValid)
            {
                technicians.Insert(technician);
                technicians.Save();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Mode = "Add";
                return View("Edit", technician);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var technician = technicians.Get(id);
            if (technician == null)
            {
                return NotFound();
            }
            ViewBag.Mode = "Edit";
            return View(technician);
        }

        [HttpPost]
        public ActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                technicians.Update(technician);
                technicians.Save();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Mode = "Edit";
                return View(technician);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var technician = technicians.Get(id);
            if (technician == null)
            {
                return NotFound();
            }
            return View(technician);
        }

        [HttpPost]
        public ActionResult Delete(Technician technician)
        {
            technician = technicians.Get(technician.TechnicianID);
            if (technician == null)
            {
                return NotFound();
            }

            var queryOptions = new QueryOptions<Incident>();
            queryOptions.Where = i => i.TechnicianID == technician.TechnicianID;

            var incidents = this.incidents.List(queryOptions);
            foreach (var incident in incidents)
            {
                incident.TechnicianID = null;
                this.incidents.Update(incident);
            }
            this.incidents.Save();

            technicians.Delete(technician);
            technicians.Save();
            return RedirectToAction("List");
        }
    }
}
