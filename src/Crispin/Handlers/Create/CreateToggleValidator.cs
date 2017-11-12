using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure;
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

		public async Task<ICollection<string>> Validate(CreateToggleRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Name))
				return new[] { "The Name property must be filled out" };

			var toggles = await GetExistingToggles();

			if (toggles.Any(tv => tv.Name.EqualsIgnore(request.Name)))
				return new[] { $"The Name '{request.Name}' is already in use" };

			return Array.Empty<string>();
		}

		private async Task<IEnumerable<ToggleView>> GetExistingToggles()
		{
			using (var session = await _storage.BeginSession())
			{
				var view = await session.LoadProjection<AllToggles>();
				return view.Toggles;
			}
		}
	}
}
