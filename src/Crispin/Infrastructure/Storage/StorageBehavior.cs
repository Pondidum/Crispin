using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Crispin.Infrastructure.Storage
{
	public class StorageBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IStorageSession _session;

		public StorageBehavior(IStorageSession session)
		{
			_session = session;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			try
			{
				await _session.Open();

				var result = await next();

				await _session.Commit();

				return result;
			}
			catch (Exception)
			{
				await _session.Abort();
				throw;
			}
		}
	}
}
