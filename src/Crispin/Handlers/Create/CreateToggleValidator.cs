using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Infrastructure.Validation;
using Crispin.Views;

namespace Crispin.Handlers.Create
{
	public class CreateToggleValidator : IRequestValidator<CreateToggleRequest>
	{
		private readonly IStorageSession _session;

		public CreateToggleValidator(IStorageSession session)
		{
			_session = session;
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
			return await _session.QueryProjection<ToggleView>();
		}
	}
}
