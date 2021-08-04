using Sales.Common.Interfaces.Entities;
using System;

namespace Sales.Common.Entities
{
    public class CustomerEntity : ICustomerEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset Birthdate { get; set; }
    }
}
