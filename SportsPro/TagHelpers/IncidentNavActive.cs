using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SportsPro.TagHelpers
{
    /* Replacing functionality for:
    @(string.IsNullOrEmpty(Model.Filter) ? "btn-primary" : "btn-light border-0")
    @(Model.Filter == "Unassigned" ? "btn-primary" : "btn-light border-0")
    */
    [HtmlTargetElement("a", Attributes="incident-nav-active")]
    public class IncidentNavActive : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewCtx { get; set; }

        
        public string Filter { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string Filter = ViewCtx.ViewBag.Filter;
            string route = context.AllAttributes["asp-route-filter"].Value.ToString();
            output.Attributes.AppendCssClass(Filter + route);
            if ((string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(context.AllAttributes["asp-route-filter"].Value.ToString())  ) || Filter == context.AllAttributes["asp-route-filter"].Value.ToString()) 
            {
                output.Attributes.AppendCssClass("btn-primary : " + Filter);
            }
            else
            {
                output.Attributes.AppendCssClass("btn-light border-0 : " + Filter);
            }
        }
    }
}
