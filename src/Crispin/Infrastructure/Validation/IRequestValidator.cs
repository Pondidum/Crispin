using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Validation
{
	public interface IRequestValidator<T>
	{
		Task<ICollection<string>> Validate(T request);
	}
}
