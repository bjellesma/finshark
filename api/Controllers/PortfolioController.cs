using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    /// <summary>
    /// Routes pertaining to portolio which is a join table between users and stocks
    /// </summary>
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }
        /// <summary>
        /// Get the portfolio of the currently logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio(){
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        /// <summary>
        /// Create a new item to add to our portolio
        /// </summary>
        /// <param name="symbol">the symbol of the stock that we want to add</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol){
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if(stock == null){
                return NotFound("Stock Not Found");
            }

            // if the stock already exists in the user portfolio
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            if(userPortfolio.Any(stock => stock.Symbol.ToLower() == symbol.ToLower())){
                return BadRequest("Cannot add same stock to Portfolio");
            }

            // create new portfolio object
            var portfolioModel = new Portfolio{
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await _portfolioRepo.CreateAsync(portfolioModel);

            // if something goes wrong and we can't create
            if(portfolioModel == null){
                return StatusCode(500, "Unable to create");
            }

            return Ok(userPortfolio);
        }

        /// <summary>
        /// route to delete stock symbol from portfolio of currently logged in user
        /// </summary>
        /// <param name="symbol">symbol of the stock</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol){
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);

            // if the stock already exists in the user portfolio
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            // find the stock on the userportfolio
            var filteredStock = userPortfolio.Where(stock => stock.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(filteredStock.Count() == 1){
                var portfolioModel = await _portfolioRepo.DeletePortfolio(appUser,symbol);
                return Ok(portfolioModel);
            }else{
                return BadRequest("Stock is not in your portfolio");
            }
        }
    }

    
}