using Microsoft.AspNetCore.Mvc;
using SportsPro.Models.DataLayer;

namespace SportsPro.Controllers
{
    public class ValidationController : Controller
    {
        public JsonResult CheckEmail(string email, string customerId)
        {
            int id = 0;
            int.TryParse(customerId, out id);

            string error = Check.EmailExists(email, id);

            if (string.IsNullOrEmpty(error))
            {
                return Json(true);
            }
            else
            {
                return Json(error);
            }
        }
    }
}
