using Application.Utils;
using FluentValidation;
using Infrastructure.Requests.Controllers.User;
namespace Infrastructure.Validators.Account;

public class UpdateUserFilesValidator : AbstractValidator<UpdateUserFilesRequest> {
    public UpdateUserFilesValidator() {
        var maxFileSize = 5 * 1024 * 1024;

        RuleFor(x => x.Files)
            .NotNull()
            .NotEmpty()
            .WithMessage("Files are required");

        RuleForEach(x => x.Files)
            .Must(x => MimeType.GetMimeType(x.MediaRaw.FileName).Contains("image"))
            .WithMessage("File must be an image")
            .Must(x => x.MediaRaw.Length >= maxFileSize)
            .WithMessage("File size must be less or equal than 5MB");
        RuleFor(x => x.FilesToDelete)
            .NotNull()
            .WithMessage("FilesToDelete are required (can be empty)");

    }
}