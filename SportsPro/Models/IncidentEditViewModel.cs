using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace SportsPro.Models
{
    public class IncidentEditViewModel
    {
        public Incident Incident { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }
        public List<Technician> Technicians { get; set; }
        public string Mode { get; set; }
    }
}

