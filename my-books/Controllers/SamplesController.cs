using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SamplesController : ControllerBase
    {
        // GET: api/<SamplesController>
        [HttpGet]
        [Authorize(Roles = "Users")]
        public string Get()
        {
            return "Hello from the user side";
        }

        // GET api/<SamplesController>/5
        [HttpGet("{id}")]
        [Authorize(Roles ="Admin")]
        public string Get(int id)
        {
            return "Hello from Admin Side";
        }

        // POST api/<SamplesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SamplesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SamplesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
