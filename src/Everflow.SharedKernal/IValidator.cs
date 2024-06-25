namespace Everflow.SharedKernal;

public interface IValidator<in T>
{
    public Result<bool, Error> Validate(T value);
}