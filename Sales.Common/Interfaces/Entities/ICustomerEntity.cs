using System;

namespace Sales.Common.Interfaces.Entities
{
    public interface ICustomerEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTimeOffset Birthdate { get; set; }
    }
}
