using ONIK_BANK.Models;

namespace ONIK_BANK.IService
{
    public interface ICustomerService
    {
        public void CreateCustomer(Customer customer);
        List<Customer> GetCustomers();
        Customer GetCustomerById(string id);
        public void DeleteCustomer(Customer customer);
        Customer GetCustomerByName(string name);
        List<Customer> GetAllCustomers();
    }
    //git push is done
}
