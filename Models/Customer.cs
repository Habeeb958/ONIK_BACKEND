using System.ComponentModel.DataAnnotations;

namespace ONIK_BANK.Models
{
    public class Customer
    {
        [Key]
        public string Customer_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BVN { get; set; }
        public decimal AccountBalance { get; set; }
        public string NextOfKin { get; set; }
    }
}
