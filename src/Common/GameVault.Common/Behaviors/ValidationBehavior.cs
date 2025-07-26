﻿using FluentValidation;
using GameVault.Common.Interfaces.CQRS.Commands;
using MediatR;

namespace GameVault.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        private IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = 
                await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)).ToList());

            var failures =
                validationResults.Where(vr => vr.Errors.Any())
                .SelectMany(vr => vr.Errors)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
