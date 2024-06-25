using System.Linq.Expressions;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Auth;

public record Login(string Username, string Password)
{
    public class UsernameSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
            => x => !string.IsNullOrWhiteSpace(x);
    } 
    
    public class PasswordSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
            => x => !string.IsNullOrWhiteSpace(x);
    }
    
    public static class Errors
    {
        public static Error UsernameIsRequired => Error.Validation(
            "Login.UsernameIsRequired", "Login Username is required");
        
        public static Error PasswordIsRequired => Error.Validation(
            "Login.PasswordIsRequired", "Login Password is required");
        
        public static Error LoginFailed => Error.Failure(
            "Login.LoginFailed", "Login failed");
    }
}