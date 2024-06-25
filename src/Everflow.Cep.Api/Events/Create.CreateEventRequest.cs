using Everflow.Cep.Application.Events;
using Everflow.Cep.Application.Events.Create;

namespace Everflow.Cep.Api.Events;

public class CreateEventRequest
{
    public int CreatedByUserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Status { get; set; }
}

public static class CreateEventRequestExtensions
{
    public static CreateEventCommand ToCreateEventCommand(this CreateEventRequest request)
        => new(
            request.CreatedByUserId,
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            EventStatusDtos.FromString(request.Status));
}