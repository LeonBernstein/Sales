using Sales.Common.Interfaces.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sales.WebApi.Models
{
    public class CustomerModel : ICustomerEntity
    {
        [Required]
        [MinLength(1)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(1)]
        public string LastName { get; set; }
        [Required]
        public DateTimeOffset Birthdate { get; set; }
    }
}
