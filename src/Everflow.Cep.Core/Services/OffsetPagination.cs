using System.Linq.Expressions;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Services;

public record OffsetPagination
{
    public int Offset { get; }
    public int Limit { get; }

    private OffsetPagination(int offset, int limit)
    {
        Offset = offset;
        Limit = limit;
    }

    public static OffsetPagination Create(int offset, int limit, OffsetPaginationSettings settings)
    {
        limit = limit > settings.MaxLimit ? settings.MaxLimit : limit;
        return new OffsetPagination(offset, limit);
    }
    
    public class OffsetSpecification : Specification<int>
    {
        public override Expression<Func<int, bool>> ToExpression()
            => x => x >= 0;
    }
    
    public class LimitSpecification(OffsetPaginationSettings settings) : Specification<int>
    {
        public override Expression<Func<int, bool>> ToExpression()
            => x => x > 0 && x <= settings.MaxLimit;
    }

    public static class Errors
    {
        public static Error OffsetMustBeZeroOrMore => Error.Validation("OffsetPagination.OffsetMustBeZeroOrMore", "OffsetPagination Offset must be zero or more");
        public static Error LimitIsOutOfRange => Error.Validation("OffsetPagination.LimitIsOutOfRange", "OffsetPagination Limit is out of range");
    }
}