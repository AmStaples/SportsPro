using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using SportsPro.Models;
using SportsPro.Models.DataLayer;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace SportsPro.TagHelpers
{
    public class SelectListTagHelper : TagHelper
    {
        private IRepository<Technician> technicians;
        private IRepository<Customer> customers;
        private IRepository<Country> countries;
        private IRepository<Product> products;

        public string EntityType { get; set; }

        public int SelectedId { get; set; }

        public SelectListTagHelper(IRepository<Technician> technicians, IRepository<Customer> customers, IRepository<Country> countries, IRepository<Product> products)
        {
            this.technicians = technicians;
            this.customers = customers;
            this.countries = countries;
            this.products = products;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AppendCssClass("form-select");

            var entries = new List<(string Id, string Name)>();

            entries.Add(("", $"Select a {EntityType}..."));

            switch(EntityType)
            {
                case "technician":
                    foreach (var technician in technicians.List(new QueryOptions<Technician>()))
                        entries.Add((technician.TechnicianID.ToString(), technician.Name));
                    break;
                case "customer":
                    foreach (var customer in customers.List(new QueryOptions<Customer>()))
                        entries.Add((customer.CustomerID.ToString(), customer.FullName));
                    break;
                case "country":
                    foreach (var country in countries.List(new QueryOptions<Country>()))
                        entries.Add((country.CountryID, country.Name));
                    break;
                case "product":
                    foreach (var product in products.List(new QueryOptions<Product>()))
                        entries.Add((product.ProductID.ToString(), product.Name));
                    break;
            }

            output.TagName = "select";
            output.TagMode = TagMode.StartTagAndEndTag;

            foreach (var entry in entries)
            {
                TagBuilder option = new TagBuilder("option");
                option.Attributes.Add("value", entry.Id);
                option.InnerHtml.Append(entry.Name);
                output.Content.AppendHtml(option);
            }
        }
    }
}
