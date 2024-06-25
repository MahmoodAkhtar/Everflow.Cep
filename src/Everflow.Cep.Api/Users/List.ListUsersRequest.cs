using FastEndpoints;

namespace Everflow.Cep.Api.Users;

public class ListUsersRequest
{
    public int Limit { get; set; }
    
    public int Offset { get; set; }
}