using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.WebApi.Controllers
{
    [ApiController, Route("api/[controller]/")]
    public class CustomersController : SalesBaseController
    {
        public CustomersController(ILogger<SalesBaseController> logger)
            : base(logger)
        {

        }

        //[HttpPost, Route("AddCustomer")]
        //public async Task<IActionResult> AddCustomer([])
    }
}
