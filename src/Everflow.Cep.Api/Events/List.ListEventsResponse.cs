namespace Everflow.Cep.Api.Events;

public record ListEventsResponse(IEnumerable<EventRecord> Events);