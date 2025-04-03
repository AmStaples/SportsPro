using System.Linq;
using SportsPro.Models.DataLayer;

namespace SportsPro.Models
{
    public static class Check
    {
        public static string EmailExists(IRepository<Customer> customers, string email, int customerId)
        {
            var queryOptions = new QueryOptions<Customer>
            {
                Where = c => c.Email == email && c.CustomerID != customerId
            };

            var duplicates = customers.List(queryOptions);

            return (duplicates.Any()) ? "Email address already in use." : "";
        }
    }
}

