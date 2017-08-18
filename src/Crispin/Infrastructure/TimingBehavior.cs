using System.Diagnostics;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crispin.Infrastructure
{
	public class TimingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<TimingBehavior<TRequest, TResponse>> _logger;

		public TimingBehavior(ILogger<TimingBehavior<TRequest, TResponse>> logger)
		{
			_logger = logger;
		}
		
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
		{
			var sw = new Stopwatch();
			sw.Start();
			var response = await next();
			sw.Stop();

			_logger.LogDebug("Handling {request} took {elapsed}ms", request.GetType().Name, sw.ElapsedMilliseconds);
			return response;
		}
	}
}
