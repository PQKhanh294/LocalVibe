using FluentValidation;
using LocalVibe.API.DTOs.Votes;

namespace LocalVibe.API.Validators;

public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(x => x.VoteType)
            .IsInEnum().WithMessage("VoteType không hợp lệ. Các giá trị cho phép: Up, Down.");

        RuleFor(x => x.VoterToken)
            .NotEmpty().WithMessage("VoterToken không được để trống.")
            .MaximumLength(200).WithMessage("VoterToken tối đa 200 ký tự.");
    }
}
