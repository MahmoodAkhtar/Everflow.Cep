using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Services;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.List;

public class ListUsersHandler : IRequestHandler<ListUsersQuery, Result<IEnumerable<UserDto>, Error>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<ListUsersQuery> _validator;
    private readonly OffsetPaginationSettings _offsetPaginationSettings;

    public ListUsersHandler(IUsersRepository usersRepository, IValidator<ListUsersQuery> validator,
        OffsetPaginationSettings offsetPaginationSettings)
    {
        _usersRepository = usersRepository;
        _validator = validator;
        _offsetPaginationSettings = offsetPaginationSettings;
    }

    public async Task<Result<IEnumerable<UserDto>, Error>> Handle(ListUsersQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var offsetLimit = OffsetPagination.Create(request.Offset, request.Limit, _offsetPaginationSettings);
        var listResult = await _usersRepository.ListAsync(offsetLimit.Offset, offsetLimit.Limit, cancellationToken);

        return listResult.Match<Result<IEnumerable<UserDto>, Error>>(
            users => users.Select(UserDto.FromUser).ToList(),
            error => error);
    }
}