namespace Everflow.SharedKernal;

public readonly record struct Result<TValue, TError> {
    public readonly TValue? Value;
    public readonly TError? Error;
    
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(TValue value)
    {
        Value = value;
        Error = default;
        IsSuccess = true;
    }
    
    private Result(TError error)
    {
        Value = default;
        Error = error;
        IsSuccess = false;
    }

    public static Result<TValue, TError> Success(TValue value) => new(value);
    public static Result<TValue, TError> Failure(TError error) => new(error);

    public static implicit operator Result<TValue, TError>(TValue value) => Success(value);
    public static implicit operator Result<TValue, TError>(TError error) => Failure(error);

    // TODO: Should this be an extension method ???
    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure) 
        => IsSuccess ? success(Value!) : failure(Error!);
}

// TODO: Can you write a Combine function extension method for Result<TValue, TError>
// TODO: Can you write a Bind function extension method for Result<TValue, TError>
// TODO: Can you write a Tap function extension method for Result<TValue, TError>
// TODO: Can you write a Map function extension method for Result<TValue, TError>
