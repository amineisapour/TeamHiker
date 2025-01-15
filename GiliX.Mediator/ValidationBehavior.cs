using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GiliX.Mediator
{
	public class ValidationBehavior<TRequest, TResponse> : object,
        MediatR.IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse>
    {
        public ValidationBehavior(System.Collections.Generic.IEnumerable<FluentValidation.IValidator<TRequest>> validators)
        {
            Validators = validators;
        }

        protected System.Collections.Generic.IEnumerable<FluentValidation.IValidator<TRequest>> Validators { get; }

        public async System.Threading.Tasks.Task<TResponse> Handle(
            TRequest request, System.Threading.CancellationToken cancellationToken,
            MediatR.RequestHandlerDelegate<TResponse> next)
        {
            if (Validators.Any())
            {
                var context =
                    new FluentValidation.ValidationContext<TRequest>(request);

                var validationResults =
                    await System.Threading.Tasks.Task.WhenAll
                        (Validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures =
                        validationResults
                            .SelectMany(current => current.Errors)
                            .Where(current => current != null)
                            .ToList()
                    ;

                if (failures.Count != 0)
                {
                    throw new FluentValidation.ValidationException(errors: failures);
                }
            }

            return await next();
        }

        Task<TResponse> IPipelineBehavior<TRequest, TResponse>.Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
