using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alba;
using Crispin.Conditions;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class RouteTests : IAsyncLifetime
	{
		private const string Name = "the-toggle";
		private const string ID = "a7fa0716-ec0e-4582-a22c-fe9673ed52ca";
		private const string Tag = "environment:dev";

		private readonly SystemUnderTest _system;
		private readonly Toggle _toggle;
		private readonly InMemoryStorage _storage;

		public RouteTests()
		{
			_storage = new InMemoryStorage();
			_storage.RegisterProjection<ToggleView>();
			_storage.RegisterAggregate<ToggleID, Toggle>();

			_system = SystemUnderTest.ForStartup<Startup>();
			_system.ConfigureServices(services => services.AddSingleton<IStorage>(_storage));

			var editor = EditorID.Parse("RouteTests");
			_toggle = Toggle.CreateNew(
				editor,
				Name,
				toggleID: ToggleID.Parse(Guid.Parse(ID))
			);

			_toggle.AddTag(editor, "test");
			_toggle.AddCondition(editor, new Dictionary<string, object>
			{
				{ ConditionBuilder.TypeKey, "enabled" }
			});
		}

		public async Task InitializeAsync()
		{
			using (var session = _storage.CreateSession())
				await session.Save(_toggle);
		}

		public Task DisposeAsync() => Task.Run(() => _system.Dispose());

		[Theory]
		[InlineData("GET", "/toggles")]
		//
		[InlineData("GET", "/toggles/id/" + ID)]
		[InlineData("GET", "/toggles/name/" + Name)]
		//
		[InlineData("GET", "/toggles/id/" + ID + "/tags")]
		[InlineData("GET", "/toggles/name/" + Name + "/tags")]
		//
		[InlineData("PUT", "/toggles/id/" + ID + "/tags/environmnet:test")]
		[InlineData("PUT", "/toggles/name/" + Name + "/tags/environmnet:test")]
		//
		[InlineData("DELETE", "/toggles/id/" + ID + "/tags/" + Tag)]
		[InlineData("DELETE", "/toggles/name/" + Name + "/tags/" + Tag)]
		//
		[InlineData("GET", "/toggles/id/" + ID + "/conditions")]
		[InlineData("GET", "/toggles/name/" + Name + "/conditions")]
		//
		[InlineData("GET", "/toggles/id/" + ID + "/conditions/0")]
		[InlineData("GET", "/toggles/name/" + Name + "/conditions/0")]
		public Task Route_works(string method, string url) => _system.Scenario(_ =>
		{
			_.Context.HttpMethod(method);
			_.Context.RelativeUrl(url);

			_.StatusCodeShouldBeOk();
			_.ContentTypeShouldBe("application/json; charset=utf-8");
		});
	}
}
