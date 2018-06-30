using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Infrastructure.Validation;

namespace Crispin.Handlers.AddCondition
{
	public class AddConditionValidator : IRequestValidator<AddToggleConditionRequest>
	{
		private readonly IConditionBuilder _builder;

		public AddConditionValidator(IConditionBuilder builder)
		{
			_builder = builder;
		}

		public async Task<ICollection<string>> Validate(AddToggleConditionRequest request)
		{
			request.Condition = _builder.CreateCondition(request.Properties);
			return Array.Empty<string>();
		}
	}
}
