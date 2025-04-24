using Microsoft.AspNetCore.Mvc;
using System;
namespace SportsPro.Views.Shared.Components
{
    public class CopyrightInfo : ViewComponent
    {
        private int date = DateTime.Now.Year;

        public IViewComponentResult Invoke() => View("_CopyrightInfo.cshtml", date);
    }
}
