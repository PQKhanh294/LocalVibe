using FluentValidation;
using LocalVibe.API.DTOs.Posts;
using LocalVibe.API.Entities.Enums;

namespace LocalVibe.API.Validators;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    public CreatePostRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề không được để trống.")
            .MaximumLength(150).WithMessage("Tiêu đề tối đa 150 ký tự.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Mô tả tối đa 1000 ký tự.")
            .When(x => x.Description is not null);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Vĩ độ phải trong khoảng -90 đến 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Kinh độ phải trong khoảng -180 đến 180.");

        RuleFor(x => x.Tag)
            .IsInEnum().WithMessage("Tag không hợp lệ. Các giá trị cho phép: Food, ScenicRoute, Coffee.");

        RuleFor(x => x.Image)
            .Must(file =>
            {
                if (file is null) return true;
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                return AllowedExtensions.Contains(ext);
            })
            .WithMessage("Chỉ chấp nhận file ảnh .jpg, .jpeg, .png, .webp.")
            .Must(file => file is null || file.Length <= MaxFileSizeBytes)
            .WithMessage("Dung lượng ảnh tối đa 5MB.");
    }
}
