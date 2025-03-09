using CustomTestCreator.Clients.Application.Tasks.Commands.UploadPhotos;
using CustomTestCreator.Core.Application.Validation;
using CustomTestCreator.SharedKernel;
using FluentValidation;

namespace CustomTestCreator.Clients.Application.Validators
{
    public class UploadFilesToTasksCommandValidator : AbstractValidator<UploadFilesToTasksCommand>
    {
        public UploadFilesToTasksCommandValidator()
        {
            RuleFor(i => i.TaskIds).NotEmpty().WithError(Errors.General.ValueIsRequired("Task ids"));

            RuleFor(f => f.Files).NotEmpty().WithError(Errors.General.ValueIsRequired("File list"));
            RuleForEach(f => f.Files).SetValidator(new UploadFileDtoValidator());
        }
    }
}
