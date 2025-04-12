using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;

namespace SportsPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ValidationController : Controller
    {
        private Repository<Customer> customers;

        public ValidationController(SportsProContext context)
        {
            customers = new Repository<Customer>(context);
        }

        public JsonResult CheckEmail(string email, string customerId, [FromServices] IRepository<Customer> cRep)
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
