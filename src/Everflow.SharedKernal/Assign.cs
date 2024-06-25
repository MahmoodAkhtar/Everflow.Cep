namespace Everflow.SharedKernal;

public static class Assign
{
    public static T OrThrowArgumentException<T>(T value, Specification<T> specification, Error error)
        => specification.IsSatisfiedBy(value)
            ? value
            : throw new ArgumentException(error.Description, error.Code);

    public static T OrAction<T>(T value, Specification<T> specification, Action<T> action)
    {
        if (!specification.IsSatisfiedBy(value)) action(value);
        return value;
    }
}