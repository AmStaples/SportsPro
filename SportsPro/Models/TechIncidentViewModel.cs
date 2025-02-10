using System.Collections.Generic;

namespace SportsPro.Models
{
    public class TechIncidentViewModel
    {
        public Technician Technician { get; set; }
        public List<Incident> Incidents { get; set; }
    }
}
