using System;
using System.Threading.Tasks;
using Alba;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class RouteTests
	{
		private const string Name = "the-toggle";
		private const string ID = "a7fa0716-ec0e-4582-a22c-fe9673ed52ca";
		private const string Tag = "environment:dev";

		private readonly SystemUnderTest _system;

		public RouteTests()
		{
			var storage = new InMemoryStorage();
			storage.RegisterProjection(new AllTogglesProjection());
			storage.RegisterBuilder(Toggle.LoadFrom);

			_system = SystemUnderTest.ForStartup<Startup>();
			_system.ConfigureServices(services => services.AddSingleton<IStorage>(storage));

			var editor = EditorID.Parse("RouteTests");
			var toggle = Toggle.CreateNew(
				editor,
				Name,
				toggleID: ToggleID.Parse(Guid.Parse(ID))
			);

			toggle.AddTag(editor, "test");

			using (var session = storage.BeginSession().Result)
				session.Save(toggle).Wait();
		}

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
		public Task Route_works(string method, string url) => _system.Scenario(_ =>
		{
			_.Context.HttpMethod(method);
			_.Context.RelativeUrl(url);

			_.StatusCodeShouldBeOk();
			_.ContentTypeShouldBe("application/json; charset=utf-8");
		});
	}
}
