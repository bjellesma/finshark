using api.Dtos.Account;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserRepository _userRepo;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IUserRepository userRepo){
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser{
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };
                // create async will automatically hash and salt the passwords
                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if(createUser.Succeeded){
                    // add user to role once created
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded){
                        return Ok(
                            new UserDto{
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }else{
                        return StatusCode(500, roleResult.Errors);
                    }
                }else{
                    return StatusCode(500, createUser.Errors);
                }
            } catch (Exception e){
                return StatusCode(500, e);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            // if user can't be found
            if(user == null){
                return Unauthorized("No User has been found.");
            }

            // the bool third param specifies if we're locking out the user after a certain amount of invalid attempts
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(!result.Succeeded){
                return Unauthorized("That username or password is incorrect. Please try again");
            }

            return Ok(
                new UserDto{
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
            
        }
        // the authorize annotation will give a 401 response if you send an invalid or blank token
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] UserQueryObject query){
            var users = await _userRepo.GetAllAsync(query);
            var usersDto = users.Select(user => user.ToUserDto());

            return Ok(usersDto);
        }
    }

}