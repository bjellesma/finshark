using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add new Portfolio object to the database
        /// </summary>
        /// <param name="portfolio">portfolio object. Definition in interface</param>
        /// <returns></returns>
        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        /// <summary>
        /// delete portfolio item from database
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            // search portfolios to find portfolio of current user with specified symbol
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(portfolio => portfolio.AppUserId == appUser.Id && portfolio.Stock.Symbol.ToLower() == symbol.ToLower());

            if(portfolioModel == null){
                return null;
            }

            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(user => user.AppUserId == user.AppUserId).Select(stock => new Stock{
                 Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
            }).ToListAsync();
        }
    }
}