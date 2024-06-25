using Everflow.Cep.Core.Auth;
using Everflow.Cep.Core.Interfaces;
using Everflow.Cep.Core.Services;
using Everflow.Cep.Core.Users;
using Everflow.SharedKernal;
using FluentAssertions;
using Moq;

namespace Everflow.Cep.Core.Tests.Services;

public class AuthServiceTests
{
    [AutoMoqData]
    [Theory]
    public async Task AuthenticateLoginAsync_When_UsernameDoesntExit_Then_ReturnsLoginFailedError(
        Mock<IUsersRepository> mockUsersRepository,
        Login login,
        User user,
        CancellationToken cancellationToken)
    {
        // Arrange
        user = null;
        mockUsersRepository.Setup(x => x.GetByEmailAsync(login.Username, cancellationToken))
            .ReturnsAsync(Result<User?, Error>.Success(user));

        var authService = new AuthService(mockUsersRepository.Object);
        var expected = Result<bool, Error>.Failure(Login.Errors.LoginFailed);

        // Act
        var actual = await authService.AuthenticateLoginAsync(login, cancellationToken);
        
        // Assert
        actual.Should().Be(expected);
    }
    
    [AutoMoqData]
    [Theory]
    public async Task AuthenticateLoginAsync_When_UserRepositoryHasUnexpectedError_Then_ReturnsUnexpectedError(
        Mock<IUsersRepository> mockUsersRepository,
        Login login,
        string code,
        string description,
        CancellationToken cancellationToken)
    {
        // Arrange
        var unexpectedError = Error.Unexpected(code, description);
        mockUsersRepository.Setup(x => x.GetByEmailAsync(login.Username, cancellationToken))
            .ReturnsAsync(Result<User?, Error>.Failure(unexpectedError));

        var authService = new AuthService(mockUsersRepository.Object);
        var expected = Result<bool, Error>.Failure(unexpectedError);

        // Act
        var actual = await authService.AuthenticateLoginAsync(login, cancellationToken);
        
        // Assert
        actual.Should().Be(expected);
    }
    
    [AutoMoqData]
    [Theory]
    public async Task AuthenticateLoginAsync_When_PasswordDoesntMatch_Then_ReturnsLoginFailedError(
        Mock<IUsersRepository> mockUsersRepository,
        Login login,
        CancellationToken cancellationToken)
    {
        // Arrange
        var differentUser = new User(1, "some name", "some1@example.com", "DoNotMatch");
        mockUsersRepository.Setup(x => x.GetByEmailAsync(login.Username, cancellationToken))
            .ReturnsAsync(Result<User?, Error>.Success(differentUser));

        var authService = new AuthService(mockUsersRepository.Object);
        var expected = Result<bool, Error>.Failure(Login.Errors.LoginFailed);

        // Act
        var actual = await authService.AuthenticateLoginAsync(login, cancellationToken);
        
        // Assert
        actual.Should().Be(expected);
    } 
    
    [AutoMoqData]
    [Theory]
    public async Task AuthenticateLoginAsync_When_UsernameExistsAndPasswordMatches_Then_ReturnsTrueResult(
        Mock<IUsersRepository> mockUsersRepository,
        CancellationToken cancellationToken)
    {
        // Arrange
        var user = new User(1, "some name", "some1@example.com", "SamePassword");
        var login = new Login(user.Email, user.Password);
        mockUsersRepository.Setup(x => x.GetByEmailAsync(login.Username, cancellationToken))
            .ReturnsAsync(Result<User?, Error>.Success(user));

        var authService = new AuthService(mockUsersRepository.Object);
        var expected = Result<bool, Error>.Success(true);

        // Act
        var actual = await authService.AuthenticateLoginAsync(login, cancellationToken);
        
        // Assert
        actual.Should().Be(expected);
    }      
}