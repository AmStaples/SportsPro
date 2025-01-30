using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace SportsPro.Models
{
    public class IncidentEditViewModel
    {
        public Incident Incident { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> Products { get; set; }
        public List<SelectListItem> Technicians { get; set; }
        public string Mode { get; set; }
    }
}

