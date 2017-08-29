using System.Collections.Generic;

namespace Crispin.Infrastructure.Validation
{
	public interface IRequestValidator<T>
	{
		ICollection<string> Validate(T request);
	}
}
