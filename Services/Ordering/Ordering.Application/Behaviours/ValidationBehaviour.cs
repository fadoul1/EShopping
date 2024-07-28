using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviours;

/// <summary>
/// This will collect all fluent validators and run before handler
/// IValidator, will return all the classes which implement this under _validators
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <param name="_validators"></param>
public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            //This runs all the validation rules one by one returns the validation result
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            //Now, need to check for any failures
            var failures = validationResults
                .SelectMany(e => e.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }

        //On success, continue the mediator pipeline for the next step
        return await next();
    }
}
