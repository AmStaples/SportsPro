using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;
using System.Collections.Generic;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("select", Attributes ="my-select")]
    public class MySelectTagHelper : TagHelper
    {

        [HtmlAttributeName("my-list-items")]
        public IEnumerable<SelectListItem> Items { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            foreach (SelectListItem item in Items)
            {
                output.Content.AppendHtml($"<option value=\"{item.Value}\">{item.Text}</option>");
            }
            
        }

    }
}
