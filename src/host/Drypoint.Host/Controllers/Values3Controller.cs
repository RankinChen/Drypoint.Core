using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drypoint.Application.Custom.Demo;
using Drypoint.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Drypoint.Host.Controllers
{
    [ApiVersion("3")]
    [Route("api/[controller]")]
    [ApiController]
    public class Values3Controller : ControllerBase
    {
        public readonly IDemoAppService demoAppService;

        public Values3Controller(IDemoAppService _demoAppService)
        {
            demoAppService = _demoAppService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var result = demoAppService.GetAll();

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
