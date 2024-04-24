// the point of a repository class is to abstract away any functionality that will be acting on the database
using System.Reflection;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        // application db context is a dependency provided in this contructor only which is a dependency injection design pattern
        // the advantage of this approach is that the instantiation of the applicationdbcontext is now abstracted away and decoupled from this class
        public StockRepository(ApplicationDBContext context){
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            // add will just add the data to the buffer on the entity framework while save changes will actually run the sql
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            // check if exists
            if(stockModel == null){
                return null;
            }
            // remove is not an async function because it does not allow for a database change, just a state transaction
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            // Get the properties of QueryObject using system.reflection
            PropertyInfo[] properties = typeof(QueryObject).GetProperties();
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();
            // test if the string is not null or empty


            // Iterate over each property in query object and use the query param in a where clause
            foreach (PropertyInfo property in properties)
            {
                // Get the value of the property
                var value = property.GetValue(query) as string;

                // check the queryType
                var queryType = property.GetCustomAttribute<QueryType>();
                if (queryType != null && !string.IsNullOrWhiteSpace(value)){
                    
                    
                    switch(queryType.Type){
                        case "Where":
                        // Apply contains filter
                        stocks = stocks.Where(stock => EF.Property<string>(stock, property.Name).Contains(value));
                        break;
                    case "OrderBy":
                        // Apply ordering
                        if (query.IsDescending)
                        {
                            stocks = stocks.OrderByDescending(stock => stock.Symbol);
                        }
                        else
                        {
                            stocks = stocks.OrderBy(stock => stock.Symbol);
                        }
                        break;
                    }
                }
            }
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            
            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        

        public async Task<Stock?> GetByIdAsync(int id)
        {
             var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);

            if(stock == null){
                // Not Found is provided by the Entity Framework
                return null;
            }
            return stock;
        }

        /// <summary>
        /// Repo method to go to the databaase and get the stock by the given symbol
        /// </summary>
        /// <param name="symbol">string symbol</param>
        /// <returns></returns>
        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(stock => stock.Symbol == symbol);
        }

        public Task<bool> stockExists(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null){
                return null;
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();
            return stockModel;
        }
    }
}