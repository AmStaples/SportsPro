using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using SportsPro.Models;

public class TechIncidentController : Controller
{
    private SportsProContext context { get; set; }

    public TechIncidentController(SportsProContext ctx)
    {
        context = ctx;
    }

    public IActionResult SelectTechnician()
    {
        var technicians = context.Technicians.ToList();
        return View("SelectTechnician", technicians);
    }

    [HttpPost]
    public IActionResult SetTechnician(int technicianID)
    {
        if (technicianID == 0)
        {
            ModelState.AddModelError("", "You must select a technician.");
            var technicians = context.Technicians.ToList();
            return View("SelectTechnician", technicians);
        }


        HttpContext.Session.SetInt32("TechnicianID", technicianID);

        return RedirectToAction("List", new { technicianID });
    }

    public IActionResult List(int? technicianID)
    {
        if (!technicianID.HasValue)
        {
            technicianID = HttpContext.Session.GetInt32("TechnicianID");
        }

        if (!technicianID.HasValue || technicianID.Value == 0)
        {
            return RedirectToAction("SelectTechnician");
        }

        var technician = context.Technicians.Find(technicianID.Value);
        if (technician == null)
        {
            return NotFound();
        }

        var incidents = context.Incidents
            .Where(i => i.TechnicianID == technicianID.Value && i.DateClosed == null)
            .Include(i => i.Customer) 
            .Include(i => i.Product)   
            .ToList();


        var viewModel = new TechIncidentViewModel
        {
            Technician = technician,
            Incidents = incidents
        };

        return View(viewModel);
    }


    public IActionResult Edit(int id)
    {
        var incident = context.Incidents.Find(id);
        if (incident == null)
        {
            return NotFound();
        }

        return View(incident);
    }

    [HttpPost]
    public IActionResult Edit(Incident incident)
    {
        if (ModelState.IsValid)
        {
            var existingIncident = context.Incidents.Find(incident.IncidentID);
            if (existingIncident == null)
            {
                return NotFound();
            }

            incident.CustomerID = existingIncident.CustomerID;

            var customerExists = context.Customers.Any(c => c.CustomerID == incident.CustomerID);
            if (!customerExists)
            {
                ModelState.AddModelError("", "The selected customer does not exist.");
                return View(incident);
            }

            existingIncident.Description = incident.Description;
            existingIncident.DateClosed = incident.DateClosed;

            context.SaveChanges();
            return RedirectToAction("List", new { technicianID = existingIncident.TechnicianID });
        }
        return View(incident);
    }
}
