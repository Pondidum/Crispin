using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests
{
	public class RouteTests : IDisposable
	{
		private readonly TestServer _host;

		public RouteTests()
		{
			_host = new TestServer(WebHost
				.CreateDefaultBuilder()
				.UseStartup<Startup>());
		}

		[Fact]
		public async Task When_getting_the_api_root()
		{
			using (var client = _host.CreateClient())
			{
				var response = await client.GetAsync("/");
				var content = await response.Content.ReadAsStringAsync();

				content.ShouldBe("Crispin Api");
			}
		}

		[Fact]
		public async Task When_hitting_the_toggles_controller()
		{
			using (var client = _host.CreateClient())
			{
				var body = JsonConvert.SerializeObject(new { Name = "toggle-1", Description = "first toggle" });
				var response = await client.PostAsync("/toggles", new StringContent(body, Encoding.UTF8, "application/json"));
				var location = response.Headers.Location.ToString();

				response.ShouldSatisfyAllConditions(
					() => response.StatusCode.ShouldBe(HttpStatusCode.Created),
					() => location.ShouldStartWith("/toggles/id/"));

				var allToggles = await client.GetAsync("/toggles");
				var singleToggle = await client.GetAsync(location);

				allToggles.IsSuccessStatusCode.ShouldBeTrue();
				singleToggle.IsSuccessStatusCode.ShouldBeTrue();
			}
		}

		public void Dispose()
		{
			_host.Dispose();
		}
	}
}
