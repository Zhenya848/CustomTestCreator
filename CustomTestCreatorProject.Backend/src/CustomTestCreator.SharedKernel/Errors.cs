namespace CustomTestCreator.SharedKernel;

public class Errors
{
    public class General
    {
        public static Error ValueIsInvalid(string? name = null) =>
            Error.Validation("value.is.invalid", $"{(name != null ? name : "value")} is invalid");

        public static Error NotFound(Guid? id = null) =>
            Error.NotFound("record.not.found", $"record not found{(id != null ? " for id: " + id : "")}");

        public static Error Failure(string? name = null) =>
            Error.Failure("failure", $"{(name != null ? name : "value")} is failure");

        public static Error ValueIsRequired(string? name = null) =>
            Error.Conflict("value.is.required", $"{(name != null ? name : "value")} is required");
    }
}