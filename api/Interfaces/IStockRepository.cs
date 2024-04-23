using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();

        // we are using the ? on stock because it will be possible to pass a null value
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto);

        Task<Stock?> DeleteAsync(int id);
    }
}