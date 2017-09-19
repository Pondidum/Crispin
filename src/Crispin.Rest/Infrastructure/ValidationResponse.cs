using System.Collections.Generic;

namespace Crispin.Rest.Infrastructure
{
	public class ValidationResponse
	{
		public string Exception { get; set; }
		public IEnumerable<string> Messages { get; set; }
	}
}