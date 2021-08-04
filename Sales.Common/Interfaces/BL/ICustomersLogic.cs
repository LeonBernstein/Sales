using Sales.Common.Entities;
using System.Threading.Tasks;

namespace Sales.Common.Interfaces.BL
{
    public interface ICustomersLogic
    {
        Task AddCustomerAsync(CustomerEntity customer);
    }
}
