using ONIK_BANK.Data;
using ONIK_BANK.IService;
using ONIK_BANK.Models;

namespace ONIK_BANK.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerDbContext customerDbContext;

        public CustomerService(CustomerDbContext customerDbContext)
        {
            this.customerDbContext = customerDbContext;
        }

        public void CreateCustomer(Customer customer)
        {
            customerDbContext.Customers.Add(customer);
            customerDbContext.SaveChanges();
        }

        public void DeleteCustomer(Customer customer)
        {
            customerDbContext.Customers.Remove(customer);
            customerDbContext.SaveChanges();
        }

        public List<Customer> GetAllCustomers()
        {
            return customerDbContext.Customers.ToList();
        }

        public Customer GetCustomerById(int id)
        {
            Customer customer = customerDbContext.Customers.Find(id);
            return customer;
        }

        public Customer GetCustomerByName(string name)
        {
            var customer = customerDbContext.Customers.Where(s => s.FirstName + s.LastName + s.OtherName == name).FirstOrDefault();
            //var customer = customerDbContext.Customers.Find(name);
            return customer;
        }

        public List<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomers(int id)
        {
            throw new NotImplementedException();
        }
    }
}
