using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public abstract class StorageTests
	{
		protected IStorage Storage { get; set; }

		[Fact]
		public async Task When_a_projection_is_registered()
		{
			var projection = new AllToggles();
			Storage.RegisterProjection(projection);

			using (var session = await Storage.BeginSession())
				session.Save(Toggle.CreateNew(EditorID.Parse("test"), "Test", "no"));

			projection.Toggles.Count().ShouldBe(1);
		}

		[Fact]
		public async Task When_there_isnt_an_aggregates_registered()
		{
			using (var session = await Storage.BeginSession())
			{
				Should.Throw<NotSupportedException>(
					() => session.LoadAggregate<Toggle>(ToggleID.CreateNew()));
			}
		}

		[Fact]
		public async Task When_an_aggregate_is_registered()
		{
			Storage.RegisterBuilder(Toggle.LoadFrom);

			using (var session = await Storage.BeginSession())
			{
				Should.Throw<KeyNotFoundException>(
					() => session.LoadAggregate<Toggle>(ToggleID.CreateNew()));
			}
		}
	}
}
