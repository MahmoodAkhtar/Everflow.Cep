using FastEndpoints;
using MediatR;

namespace Everflow.Cep.Api.Events;

public class Create(IMediator mediator, FluentValidation.IValidator<CreateEventRequest> validator)
    : Endpoint<CreateEventRequest, CreateEventResponse>
{
    public override void Configure()
    {
        Post("/events");
        AllowAnonymous();
        Description(x => x
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemFE<InternalErrorResponse>(500)
            .Produces<CreateEventResponse>(201, "application/json"));
    }

    public override async Task HandleAsync(CreateEventRequest req, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid) validationResult.Errors.ForEach(AddError);
        ThrowIfAnyErrors();

        var result = await mediator.Send(req.ToCreateEventCommand(), ct);

        await result.Match<Task>(
            id => SendCreatedAtAsync<GetById>(
                new { id }, 
                new CreateEventResponse(id), 
                cancellation: ct),
            error => Task.FromException(new Exception(error.Description))
        );
    }
}