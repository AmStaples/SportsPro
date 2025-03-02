﻿using System.Collections.Generic;

namespace SportsPro.Models
{
    public class TechIncidentViewModel
    {
        public Technician Technician { get; set; }
        public Incident Incident { get; set; }
        public List<Incident> Incidents { get; set; } = null!;
    }
}
