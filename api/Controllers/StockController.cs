using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        // todo remove context
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        [HttpGet]
        // the authorize annotation will give a 401 response if you send an invalid or blank token
        [Authorize]
        // iaction result is defined on the entity framework to hold the results of an http request
        // the fromquery anotation will allow us to get url query params
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            // LINQ queries are deferred by nature meaning that the results are not returned until they're actually needed by the application
            // using ToList forces them to be retrieved at this specific point when we're calling this
            // so essentially we're bypassing the defered and saying that we want this right now
            var stocks = await _stockRepo.GetAllAsync(query);
                // select is like map in this case
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id:int}")]
        // returning one record with the FromRoute
        public async Task<IActionResult> GetById([FromRoute] int id){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
           var stock = await _stockRepo.GetByIdAsync(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        // fromBody is an annotation saying that whatever is passed in the body of the request will exist in that param
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var stockModel = stockDto.ToStockFromCreateDTO();
            stockModel = await _stockRepo.CreateAsync(stockModel);
            // This created at action is going to perform getbyid, with the param provided in the second arg, and tostockdto is going to be the final action
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto){
           // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
             var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
            if(stockModel == null){
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, QueryObject query){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var stockModel = await _stockRepo.DeleteAsync(id);
            if(stockModel == null){
                return NotFound();
            }
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto());

            // could also use return nocontent to give a 200 with no content
            return Ok(stocks);
        }
    }

    
}