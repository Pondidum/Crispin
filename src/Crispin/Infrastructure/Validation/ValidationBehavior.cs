using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Crispin.Infrastructure.Validation
{
	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly Func<IRequestValidator<TRequest>> _validator;

		public ValidationBehavior(Func<IRequestValidator<TRequest>> validator)
		{
			_validator = validator;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
		{
			IRequestValidator<TRequest> validator = null;
			try
			{
				validator = _validator();
			}
			catch (Exception e)
			{
				// lolol on error resume next
			}

			if (validator != null)
			{
				var messages = validator.Validate(request);
				if (messages.Any())
					throw new ValidationException(messages);
			}

			return await next();
		}
	}
}
