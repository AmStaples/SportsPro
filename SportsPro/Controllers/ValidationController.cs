using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;

namespace SportsPro.Controllers
{
    public class ValidationController : Controller
    {

        public JsonResult CheckEmail(string email, string customerId, [FromServices]IRepository<Customer> cRep)
        {
            int id = 0;
            int.TryParse(customerId, out id);

            string error = Check.EmailExists(cRep, email, id);

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
