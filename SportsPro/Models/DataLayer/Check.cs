using System.Linq;

namespace SportsPro.Models.DataLayer
{
    public static class Check
    {
        public static string EmailExists(Repository<Customer> customers, string email, int customerId)
        {
            var queryOptions = new QueryOptions<Customer>();
            queryOptions.Where = c =>
                c.Email == email &&
                c.CustomerID != customerId;

            var duplicates = customers.List(queryOptions);

            return (duplicates.Count() > 0) ? "Email address already in use." : "";
        }
    }
}
