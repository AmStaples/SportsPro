using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Linq;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private SportsProContext context { get; set; }

        public TechnicianController(SportsProContext ctx)
        {
            context = ctx;
        }

        [Route("[controller]s")]
        [HttpGet]
        public ViewResult List()
        {
            var technicians = context.Technicians.OrderBy(t => t.Name).ToList();
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
                context.Technicians.Add(technician);
                context.SaveChanges();
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
            var technician = context.Technicians.Find(id);
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
                context.Technicians.Update(technician);
                context.SaveChanges();
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
            var technician = context.Technicians.Find(id);
            if (technician == null)
            {
                return NotFound();
            }
            return View(technician);
        }

        [HttpPost]
        public ActionResult Delete(Technician technician)
        {
            technician = context.Technicians.Find(technician.TechnicianID);
            if (technician == null)
            {
                return NotFound();
            }
            var incidents = context.Incidents.Where(i => i.TechnicianID == technician.TechnicianID).ToList();
            foreach (var incident in incidents)
            {
                incident.TechnicianID = null;
                context.Update(incident);
            }
            context.Remove(technician);
            context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
