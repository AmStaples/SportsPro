using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;

namespace SportsPro.Controllers
{
    public class ValidationController : Controller
    {
        private Repository<Customer> customers;

        public ValidationController(SportsProContext context)
        {
            customers = new Repository<Customer>(context);
        }

        public JsonResult CheckEmail(string email, string customerId)
        {
            int id = 0;
            int.TryParse(customerId, out id);

            string error = Check.EmailExists(customers, email, id);

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
