using System.Linq.Expressions;
using Everflow.SharedKernal;

namespace Everflow.Cep.Core.Users;

public class User
{
    public int Id { get; init; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public User() // for Ef
    {
        
    }
    
    public User(int id, string name, string email, string password) : this(name, email, password)
    {
        Id = Assign.OrThrowArgumentException(id, new IdSpecification(), Errors.IdMustBeGreaterThanZero);
    }
    
    public User(string name, string email, string password)
    {
        Name = Assign.OrThrowArgumentException(name, new NameSpecification(), Errors.NameMustBeAValidFormat);
        Email = Assign.OrThrowArgumentException(email, new EmailSpecification(), Errors.EmailMustBeAValidFormat);
        Password = Assign.OrThrowArgumentException(password, new PasswordSpecification(), Errors.PasswordMustBeAValidFormat);
    }

    public void SetName(string name)
    {
        Name = Assign.OrThrowArgumentException(name, new NameSpecification(), Errors.NameMustBeAValidFormat);
    }
    
    public void SetEmail(string email)
    {
        Email = Assign.OrThrowArgumentException(email, new EmailSpecification(), Errors.EmailMustBeAValidFormat);
    }
        
    public void SetPassword(string password)
    {
        Password = Assign.OrThrowArgumentException(password, new PasswordSpecification(), Errors.PasswordMustBeAValidFormat);
    }

    public class IdSpecification : Specification<int>
    {
        public override Expression<Func<int, bool>> ToExpression()
            => x => x > 0;
    }

    public class NameSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
            => x => !string.IsNullOrWhiteSpace(x)
                    && x.Length <= 100;
    }

    public class EmailSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
            => x =>
                !string.IsNullOrWhiteSpace(x)               // Cannot be empty
                && x.IndexOf('@') > 0                       // @ must be present AND @ cannot be the 1st 
                && x.IndexOf('@') != x.Length -1            // @ cannot be the last
                && x.IndexOf('@') == x.LastIndexOf('@');    // Must be only 1 @
    }

    public class PasswordSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
            => x =>
                !string.IsNullOrWhiteSpace(x)
                && x.Length >= 8
                && x.Length <= 12;
    }
    
    public static class Errors
    {
        public static Error NotFound(int id) => Error.NotFound(
            "User.NotFound", $"The User with the Id = {id} was not found");
    
        public static Error EmailMustBeUnique => Error.Conflict(
            "User.EmailMustBeUnique", "User Email must be unique");
    
        public static Error IdMustBeGreaterThanZero => Error.Validation(
            "User.IdMustBeGreaterThanZero", "User Id must be greater than zero");  
    
        public static Error NameMustBeAValidFormat => Error.Validation(
            "User.NameMustBeAValidFormat", "User Name must be a valid format");
    
        public static Error EmailMustBeAValidFormat => Error.Validation(
            "User.EmailMustBeAValidFormat", "User Email must be a valid format");
    
        public static Error PasswordMustBeAValidFormat => Error.Validation(
            "User.PasswordMustBeAValidFormat", "User Password must be a valid format");
        
        public static Error IsNull => Error.Validation(
            "User.IsNull", "User is null");
    }
}

