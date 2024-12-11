using Microsoft.AspNetCore.Mvc;
using ONIK_BANK.IService;
using ONIK_BANK.Models;

namespace ONIK_BANK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpPost]
        [Route("CreateNewAccount")]

        public IActionResult CreateAccount([FromBody] Customer addcustomer)
        {
            _customerService.CreateCustomer(addcustomer);
            return Ok("Account successfully created");
        }

        [HttpGet("GetCustomerBy{id}")]

        public IActionResult GetCustomerById(string id)
        {
            if (id == null)
            {
                return BadRequest("This ID is not found");
            }
            var customer = _customerService.GetCustomerById(id);
            return Ok(customer);

        }

        [HttpGet("GetAllCustomer")]

        public IActionResult GetAllCustomers()
        {
            List<Customer> customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpDelete("DeleteCustomerBy{id}")]

        public IActionResult DeleteCustomer(string id)
        {
            var getCustomer = _customerService.GetCustomerById(id);
            if (getCustomer == null)
            {
                return NotFound("ID not found");
            }
            _customerService.DeleteCustomer(getCustomer);
            return Ok("Account successfully deleted");
        }

        [HttpGet("GetCustomerByName")]

        public IActionResult GetCustomerByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("This name is not found." +
                    "Please put in the coreect name.");
            }
            var correct = _customerService.GetCustomerByName(name);
            return Ok(correct);
        }
    }
}
