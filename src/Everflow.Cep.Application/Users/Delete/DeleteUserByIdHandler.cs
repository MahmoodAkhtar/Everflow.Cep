using Everflow.Cep.Core.Interfaces;
using Everflow.SharedKernal;
using MediatR;

namespace Everflow.Cep.Application.Users.Delete;

public class DeleteUserByIdHandler : IRequestHandler<DeleteUserByIdCommand, Result<bool, Error>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<DeleteUserByIdCommand> _validator;

    public DeleteUserByIdHandler(IUsersRepository usersRepository, IValidator<DeleteUserByIdCommand> validator)
    {
        _usersRepository = usersRepository;
        _validator = validator;
    }
    
    public async Task<Result<bool, Error>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsFailure) return validationResult.Error;

        var deleteResult = await _usersRepository.DeleteByIdAsync(request.Id, cancellationToken);
        
        return deleteResult;
    }
}