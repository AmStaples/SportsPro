using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Components
{
    public class ConfirmDeletion : ViewComponent
    {
        public IViewComponentResult Invoke(int id, string name)
        {
            var model = new ConfirmDeletionViewModel { Id = id, Name = name };
            return View(model);
        }
    }
}
