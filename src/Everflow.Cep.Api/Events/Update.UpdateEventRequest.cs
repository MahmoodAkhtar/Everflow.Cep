using Everflow.Cep.Application.Events;
using Everflow.Cep.Application.Events.Update;

namespace Everflow.Cep.Api.Events;

public class UpdateEventRequest
{
    public int CreatedByUserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Status { get; set; }
}

public static class UpdateEventRequestExtensions
{
    public static UpdateEventCommand ToUpdateEventCommand(this UpdateEventRequest request, int id)
        => new(
            id,
            request.CreatedByUserId,
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            EventStatusDtos.FromString(request.Status));
}