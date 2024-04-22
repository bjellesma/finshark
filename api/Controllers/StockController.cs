using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;

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
            var stocks = _context.Stocks.ToList()
                // select is like map in this case
                .Select(s => s.ToStockDto());

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

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        // fromBody is an annotation saying that whatever is passed in the body of the request will exist in that param
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto){
            var stockModel = stockDto.ToStockFromCreateDTO();
            // add will just add the data to the buffer on the entity framework while save changes will actually run the sql
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            // This created at action is going to perform getbyid, with the param provided in the second arg, and tostockdto is going to be the final action
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto){
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);

            if(stockModel == null){
                return NotFound();
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDto());
        }
    }
}