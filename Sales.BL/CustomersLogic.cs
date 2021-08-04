using Sales.Common.Entities;
using Sales.Common.Interfaces.BL;
using Sales.Common.Interfaces.DAL;
using System.Threading.Tasks;

namespace Sales.BL
{
    public class CustomersLogic : ICustomersLogic
    {
        private readonly ICustomersEngine _customerEngine;

        public CustomersLogic(ICustomersEngine customerEngine) => _customerEngine = customerEngine;

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            await _customerEngine.AddCustomerAsync(customer);
        }
    }
}
