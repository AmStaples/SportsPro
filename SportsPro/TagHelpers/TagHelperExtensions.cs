using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace SportsPro.TagHelpers
{
    public static class TagHelperExtensions
    {

        public static void AppendCssClass(this TagHelperAttributeList list, string newCssClasses)
        {
            string oldCssClasses = list["class"]?.Value?.ToString();
            string cssClasses = (string.IsNullOrEmpty(oldCssClasses)) ? newCssClasses : $"{oldCssClasses} {newCssClasses}";
            list.SetAttribute("class", cssClasses);
        }

        public static void BuildTag(this TagHelperOutput output, string tagName, string classNames)
        {
            output.TagName = tagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AppendCssClass(classNames);
        }
    }
}
