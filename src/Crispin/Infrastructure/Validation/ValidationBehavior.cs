using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Crispin.Infrastructure.Validation
{
	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IRequestValidator<TRequest> _validator;

		public ValidationBehavior(IRequestValidator<TRequest> validator = null)
		{
			_validator = validator;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
		{
			if (_validator != null)
			{
				var messages = _validator.Validate(request);
				if (messages.Any())
					throw new ValidationException(messages);
			}

			return await next();
		}
	}
}
