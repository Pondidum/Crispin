﻿using System.Threading.Tasks;
using Crispin.Handlers.Create;
using Crispin.Infrastructure.Validation;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class CreatingToggleWithNoName : HandlerTest<CreateToggleRequest, CreateTogglesResponse>
	{
		protected override Task<CreateToggleRequest> When()
		{
			return Task.FromResult(new CreateToggleRequest(Editor, "", ""));
		}

		[Fact]
		public void It_throws_a_validation_exception_about_the_name() => Exception.ShouldBeOfType<ValidationException>();

		[Fact]
		public void The_exception_message_mentions_the_name_property() => Exception.Message.ShouldContain("Name");
	}
}
