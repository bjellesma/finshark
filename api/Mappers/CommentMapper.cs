using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel){
            return new CommentDto{
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                // use null checking to check if logged in
                CreatedBy = commentModel.AppUser?.UserName ?? "Anonymous",
                StockId = commentModel.StockId
            };
        }
        public static Comment ToCommentFromCreateDto(this CreateCommentRequestDto commentDto, int StockId){
            return new Comment{
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = StockId
            };
        }
    }
}