using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoTest.Repositories;
using SheetApi.Common.Models;
using WebApi.Authentikate;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        private IMongoTestRepository _mongoTest;
        private IAuthentikateService _authentikate;

        public ValuesController(IMongoTestRepository mongoTest, IAuthentikateService authentikate)
        {
            _mongoTest = mongoTest;
            _authentikate = authentikate;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // validate request
            var user = await _authentikate.GetUser(HttpContext);
            if (string.IsNullOrEmpty(user)) return this.UnauthorizedResponse();

            _mongoTest.Create(new Book
            {
                BookName = "Very Good Book",
                Price = (decimal)(9.99),
                Category = "Good Books",
                Author = "Good Writer"

            });
            return this.OkResponse(_mongoTest.GetBooks());
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
