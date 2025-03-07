using CustomTestCreator.Core.Application.Validation;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using FluentValidation;

namespace PetProject.Volunteers.Application.Validators
{
    public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
    {
        public UploadFileDtoValidator() 
        {
            RuleFor(n => n.FileName).Must(n => string.IsNullOrWhiteSpace(n) == false)
                .WithError(Errors.General.ValueIsRequired("File name"));

            RuleFor(n => n.ContentType).Must(n => n.Length < 10000000)
                .WithError(Errors.General.ValueIsInvalid("Content type"));
        }
    }
}
