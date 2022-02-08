using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MindUWebApi.Controllers.V2
{
    [Route("api/v2/Values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ValuesController> logger;

        public ValuesController(IConfiguration configuration, ILogger<ValuesController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet("GetTime")]
        public ActionResult<DateTime> GetTime()
        {
            return Ok(DateTime.Now);
        }

        [HttpGet("GetAppValue")]
        public ActionResult<string> GetValue()
        {
            return Ok(configuration["SettingKey"]);
        }

        [HttpGet("TestLogs")]
        public ActionResult<DateTime> TestLogs()
        {
            var date = DateTime.Now;
            logger.LogWarning("Test Log from ValuesController", date.ToString());
            return Ok(date);
        }


    }
}
