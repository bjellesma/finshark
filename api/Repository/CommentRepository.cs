using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if(commentModel == null){
                return null;
            }
            // remove is not an async function because it does not allow for a database change, just a state transaction
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            // we need to use include to go to the associated app user model because of deferred execution
            return await _context.Comments.Include(account => account.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if(comment == null){
                return null;
            }
            return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto updateDto)
        {
            var commentModel = await _context.Comments.Include(account => account.AppUser).FirstOrDefaultAsync(x => x.Id == id);

            if(commentModel == null){
                return null;
            }

            commentModel.Title = updateDto.Title;
            commentModel.Content = updateDto.Content;

            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}