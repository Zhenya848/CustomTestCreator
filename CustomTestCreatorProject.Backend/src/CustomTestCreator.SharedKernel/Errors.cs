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
    
    public static class User
    {
        public static Error AlreadyExist() =>
            Error.Validation("user.already.exist", "user with this name already exist!");
        
        public static Error NotFound() => 
            Error.NotFound("user.notfound", $"user not found");
        
        public static Error NotFound(string username) => 
            Error.NotFound("user.notfound", $"user with name {username} not found");
            
        public static Error WrongCredentials() =>
            Error.Validation("user.wrong.credentials", "user with wrong credentials");
    }
    
    public static class Token
    {
        public static Error InvalidToken()
        {
            return Error.Validation("token.is.invalid", "Your token is invalid");
        }
    }
}