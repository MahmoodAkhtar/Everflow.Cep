using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace Everflow.SharedKernal;

public readonly record struct Error
{
    public static Error None => new(string.Empty, string.Empty, ErrorType.None);
    
    public readonly string Code;
    public readonly string Description;
    public readonly ErrorType Type;

    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static Error Failure(string code, string description)
        => new(code, description, ErrorType.Failure);

    public static Error Unexpected(string code, string description)
        => new(code, description, ErrorType.Unexpected);

    public static Error Validation(string code, string description)
        => new(code, description, ErrorType.Validation);

    public static Error Conflict(string code, string description)
        => new(code, description, ErrorType.Conflict);

    public static Error NotFound(string code, string description)
        => new(code, description, ErrorType.NotFound);

    public static Error Unauthorized(string code, string description)
        => new(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string description)
        => new(code, description, ErrorType.Forbidden);

    public static Error FromException(Exception e)
        => Unexpected(e.GetType().Name, $"{e.Message} {e.StackTrace}");
}

public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    None
}