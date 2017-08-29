using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Validation
{
	public class ValidationException : Exception
	{
		public IEnumerable<string> Messages { get; }

		public ValidationException(ICollection<string> messages)
			: base(string.Join("\r\n", messages))
		{
			Messages = messages;
		}
	}
}
