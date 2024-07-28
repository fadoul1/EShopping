using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> _logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(
                e,
                "Unhandled Exception Occurred with Request Name: {requestName}, {request}",
                requestName,
                request
            );
            throw;
        }
    }
}
