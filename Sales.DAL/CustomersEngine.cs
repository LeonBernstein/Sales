using Sales.Common.Entities;
using Sales.Common.Interfaces.DAL;
using Sales.Common.Interfaces.Services;
using System.Threading.Tasks;

namespace Sales.DAL
{
    public class CustomersEngine : BaseSalesEngine, ICustomersEngine
    {
        public CustomersEngine(IJsonFileHandlerService jsonFileHandler)
            :base(jsonFileHandler) { }

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            await InsertItemAsync(EntityTypes.Customers, customer);
        }
    }
}
