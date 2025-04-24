using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SportsPro.TagHelpers
{
    /* Replacing functionality for:
    @(string.IsNullOrEmpty(Model.Filter) ? "btn-primary" : "btn-light border-0")
    @(Model.Filter == "Unassigned" ? "btn-primary" : "btn-light border-0")
    */
    [HtmlTargetElement("a", Attributes="incident-active")]
    public class IncidentNavActive : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewCtx { get; set; }

        

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string filter = ViewCtx.ViewData.GetViewDataInfo("Filter").Value.ToString();
            if (string.IsNullOrEmpty(filter) || filter == context.AllAttributes["asp-route-filter"].Value.ToString()) 
            {
                output.Attributes.AppendCssClass("btn-primary : " + filter);
            }
            else
            {
                output.Attributes.AppendCssClass("btn-light border-0 : " + filter);
            }
        }
    }
}
