using FluentValidation;
using LocalVibe.API.DTOs.Comments;

namespace LocalVibe.API.Validators;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("Tên tác giả không được để trống.")
            .MaximumLength(100).WithMessage("Tên tác giả tối đa 100 ký tự.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung bình luận không được để trống.")
            .MaximumLength(500).WithMessage("Bình luận tối đa 500 ký tự.");
    }
}
