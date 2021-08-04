using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Sales.WebApi.Controllers
{
    abstract public class SalesBaseController : ControllerBase
    {
        private readonly ILogger _logger;

        protected SalesBaseController(ILogger<SalesBaseController> logger) => _logger = logger;

        protected StatusCodeResult InternalServerError(Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
