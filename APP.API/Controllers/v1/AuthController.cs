using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APP.API.Controllers.v1
{
    /// <summary>
    /// Auth controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration Config;
        /// <summary>
        /// Auth Controller Constructor
        /// </summary>
        /// <param name="config"></param>
        public AuthController(IConfiguration config)
        {
            Config = config;
        }
    }
}