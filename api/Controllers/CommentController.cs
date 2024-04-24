using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        // we want a class var for the stock repo as well so that we can check if the stock exists
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var comments = await _commentRepo.GetAllAsync();
            // DTO (Data Transfer Object) is just a way to map the data
            var commentDto = comments.Select(c => c.ToCommentDto());

            return Ok(commentDto);
        }
        // using the semicolon to specify a type is just adding a simple type constraint
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
           var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null){
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            // check if stock exists
            if(!await _stockRepo.stockExists(stockId)){
                return BadRequest("stock does not exist");
            }

            var commentModel = commentDto.ToCommentFromCreateDto(stockId);
            // get currently logged in user
            var username = User.GetUserName();
            if(username != null){
                var appUser = await _userManager.FindByNameAsync(username);
                commentModel.AppUserId = appUser.Id;
            }
            
            commentModel = await _commentRepo.CreateAsync(commentModel);
            // This created at action is going to perform getbyid, with the param provided in the second arg, and tostockdto is going to be the final action
            return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var commentModel = await _commentRepo.UpdateAsync(id, updateDto);
            if(commentModel == null){
                return NotFound();
            }
            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            // modelstate ensures that that the data model (dtos in our case) passes all validation
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var commentModel = await _commentRepo.DeleteAsync(id);
            if(commentModel == null){
                return NotFound();
            }
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(c => c.ToCommentDto());
            return Ok(commentDto);
        }
    }
}