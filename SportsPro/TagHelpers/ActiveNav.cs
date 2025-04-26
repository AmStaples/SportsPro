using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "active-nav")]
    public class ActiveNavTagHelper : TagHelper
    {
        // replacing  " @(controller == "Home" && action == "Index" ? "active" : "") "
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewCtx {  get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string controller = context.AllAttributes["asp-controller"].Value.ToString();
            
            if (controller == ViewCtx.RouteData.Values["controller"].ToString()) 
            {
                output.Attributes.AppendCssClass("text-white");
            } 
        }
    }
}
