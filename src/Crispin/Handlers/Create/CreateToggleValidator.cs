using System;
using System.Collections.Generic;
using Crispin.Infrastructure.Validation;

namespace Crispin.Handlers.Create
{
	public class CreateToggleValidator : IRequestValidator<CreateToggleRequest>
	{
		public ICollection<string> Validate(CreateToggleRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Name))
				return new[] { "The Name property must be filled out" };

			return Array.Empty<string>();
		}
	}
}
