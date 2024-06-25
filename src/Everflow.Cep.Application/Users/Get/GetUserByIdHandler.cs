using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.Get;

public class GetUserByIdHandler(IUsersRepository _usersRepository, IValidator<GetUserByIdQuery> _validator)
    : IRequestHandler<GetUserByIdQuery, Result<UserDto?, Error>>
{
    public async Task<Result<UserDto?, Error>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return await Task.FromResult(validationResult.Error);

        var getByIdResult = await _usersRepository.GetByIdAsync(request.Id, cancellationToken);

        return getByIdResult.Match(
            user => user is null 
                ? User.Errors.NotFound(request.Id)
                : Result<UserDto?, Error>.Success(UserDto.FromUser(user)),
            error => error);
    }
}