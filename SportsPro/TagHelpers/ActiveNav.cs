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

            bool isValidAction = true;
            if (controller == "Account")
            {
                isValidAction = (context.AllAttributes["asp-action"].Value.ToString() == ViewCtx.RouteData.Values["action"].ToString());
            }

            if (isValidAction && controller == ViewCtx.RouteData.Values["controller"].ToString()) 
            {
                output.Attributes.AppendCssClass("text-white");
            } 
        }
    }
}
