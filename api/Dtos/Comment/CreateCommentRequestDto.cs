using System.ComponentModel.DataAnnotations;
using api.Validation;

namespace api.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        public const int MinLength = 5;
        public const int MaxLength = 140;

        // Length Validation is a custom validation that I've made
        [Required]
        [LengthValidation(MinLength, MaxLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [LengthValidation(MinLength, MaxLength)]
        public string Content { get; set; } = string.Empty;
    }

    
    
}