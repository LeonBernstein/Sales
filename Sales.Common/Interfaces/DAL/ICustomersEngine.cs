using Sales.Common.Entities;
using System.Threading.Tasks;

namespace Sales.Common.Interfaces.DAL
{
    public interface ICustomersEngine
    {
        Task AddCustomerAsync(CustomerEntity customer);
    }
}
