using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        // iaction result is defined on the entity framework to hold the results of an http request
        public IActionResult GetAll(){
            // LINQ queries are deferred by nature meaning that the results are not returned until they're actually needed by the application
            // using ToList forces them to be retrieved at this specific point when we're calling this
            // so essentially we're bypassing the defered and saying that we want this right now
            var stocks = _context.Stocks.ToList();

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        // returning one record with the FromRoute
        public IActionResult GetById([FromRoute] int id){
            var stock = _context.Stocks.Find(id);

            if(stock == null){
                // Not Found is provided by the Entity Framework
                return NotFound();
            }

            return Ok(stock);
        }
    }
}