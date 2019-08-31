using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Dplayer.V3
{
    [Route("api/dplayer/v3")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        // GET: api/dplayer/v3
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/dplayer/v3
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
