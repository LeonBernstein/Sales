using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sales.Common.Entities;
using Sales.Common.Interfaces.BL;
using Sales.WebApi.Models;
using System;
using System.Threading.Tasks;

namespace Sales.WebApi.Controllers
{
    [ApiController, Route("api/[controller]/")]
    public class CustomersController : SalesBaseController
    {
        private readonly ICustomersLogic _customersLogic;
        private readonly IMapper _mapper;

        public CustomersController(
            ILogger<SalesBaseController> logger,
            ICustomersLogic customersLogic,
            IMapper mapper
         ) : base(logger)
        {
            _customersLogic = customersLogic;
            _mapper = mapper;
        }

        [HttpPost, Route("AddCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                CustomerEntity customer = _mapper.Map<CustomerEntity>(model);
                await _customersLogic.AddCustomerAsync(customer);
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
