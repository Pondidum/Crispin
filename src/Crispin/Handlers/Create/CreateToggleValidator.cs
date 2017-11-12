using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Infrastructure.Storage;
using Crispin.Infrastructure.Validation;
using Crispin.Projections;

namespace Crispin.Handlers.Create
{
	public class CreateToggleValidator : IRequestValidator<CreateToggleRequest>
	{
		private readonly IStorage _storage;

		public CreateToggleValidator(IStorage storage)
		{
			_storage = storage;
		}

		public ICollection<string> Validate(CreateToggleRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Name))
				return new[] { "The Name property must be filled out" };

			using (var session = _storage.BeginSession().Result)
			{
				var view = session.LoadProjection<AllToggles>().Result.Toggles;

				if (view.Any(tv => string.Equals(tv.Name, request.Name, StringComparison.OrdinalIgnoreCase)))
				{
					return new[] { $"The Name '{request.Name}' is already in use" };
				}
			}

			return Array.Empty<string>();
		}
	}
}
