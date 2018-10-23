using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using CrispinClient.Conditions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests
{
	public class CrispinHttpClientTests : IDisposable
	{
		private readonly TestServer _server;
		private readonly CrispinHttpClient _client;
		private readonly List<string> _toggles;
		private string _stats;

		public CrispinHttpClientTests()
		{
			_server = new TestServer(new WebHostBuilder()
				.ConfigureServices(s => s.AddRouting())
				.Configure(ConfigureApp));

			_client = new CrispinHttpClient(_server.BaseAddress, _server.CreateClient());

			_toggles = new List<string>();
		}

		private void ConfigureApp(IApplicationBuilder app)
		{
			app.UseRouter(r =>
			{
				r.DefaultHandler = new RouteHandler(context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					return Task.CompletedTask;
				});

				r.MapGet("/toggles", async context =>
				{
					context.Response.Headers.Add("content-type", "application/json");
					await context.Response.WriteAsync("[" + string.Join(", ", _toggles) + "]");
				});

				r.MapPost("/stats", async context =>
				{
					using (var sr = new StreamReader(context.Request.Body))
						_stats = await sr.ReadToEndAsync();
				});
			});
		}

		[Fact]
		public async Task Toggles_are_deserialized_correctly()
		{
			_toggles.Add(@"{
  ""id"": ""57F137CA-4251-4D2D-BD40-EC798854593E"",
  ""name"": ""one"",
  ""description"": ""the first of many"",
  ""conditions"": [
  {
    ""children"": [
      { ""conditionType"": ""Enabled"", ""id"": 1 },
      { ""conditionType"": ""Disabled"", ""id"": 2 }
    ],
    ""conditionType"": ""All"",
    ""id"": 0
  }]
}");

			var toggle = (await _client.GetAllToggles()).ShouldHaveSingleItem();

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(Guid.Parse("57F137CA-4251-4D2D-BD40-EC798854593E")),
				() => toggle.Name.ShouldBe("one"),
				() => toggle.Description.ShouldBe("the first of many"),
				() => toggle.Conditions.ShouldHaveSingleItem().ShouldBeOfType<AllCondition>()
			);
		}

		[Fact]
		public async Task Statistics_are_serialised_correctly()
		{
			var stat = new Statistic
			{
				ToggleID = Guid.NewGuid(),
				Active = true,
				Timestamp = DateTime.Now,
				User = "me"
			};

			await _client.SendStatistics(new[] { stat });

			_stats.ShouldSatisfyAllConditions(
				() => _stats.ShouldContain($"\"ToggleID\":\"{stat.ToggleID}\""),
				() => _stats.ShouldContain("\"User\":\"me\""),
				() => _stats.ShouldMatch($"\"Timestamp\":\"(.*?)\""),
				() => _stats.ShouldContain("\"Active\":true"),
				() => _stats.ShouldContain("\"ConditionStates\":{}")
			);
		}

		public void Dispose()
		{
			_client.Dispose();
			_server.Dispose();
		}
	}
}
