using CustomTestCreator.SharedKernel;
using FluentValidation;
using FluentValidation.Results;

namespace CustomTestCreator.Core.Application.Validation;

public static class ValidatorsExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilderOptions, Error error)
    {
        return ruleBuilderOptions.WithMessage(error.Serialize());
    }

    public static ErrorList ValidationErrorResponse(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;
        var errors = validationErrors.Select(e => Error.Deserialize(e.ErrorMessage));
        
        return errors.ToList();
    }
}