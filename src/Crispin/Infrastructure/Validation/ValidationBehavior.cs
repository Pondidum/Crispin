using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Crispin.Infrastructure.Validation
{
	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

		public ValidationBehavior(IEnumerable<IRequestValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var validator = _validators.FirstOrDefault();

			if (validator != null)
			{
				var messages = await validator.Validate(request);
				if (messages.Any())
					throw new ValidationException(messages);
			}

			return await next();
		}
	}
}
