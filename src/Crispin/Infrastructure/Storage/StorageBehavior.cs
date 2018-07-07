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

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
		{
			try
			{
				await _session.Open();
				return await next();
			}
			finally
			{
				await _session.Commit();
			}
		}
	}
}
