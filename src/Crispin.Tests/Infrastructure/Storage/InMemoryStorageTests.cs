using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class InMemoryStorageTests
	{
		private readonly InMemoryStorage _storage;

		public InMemoryStorageTests()
		{
			_storage = new InMemoryStorage();
		}

		[Fact]
		public void When_a_projection_is_registered()
		{
			var projection = new AllToggles();
			_storage.RegisterProjection(projection);

			using (var session = _storage.BeginSession())
				session.Save(Toggle.CreateNew(() => "", "Test", "no"));

			projection.Toggles.Count().ShouldBe(1);
		}

		[Fact]
		public void When_there_isnt_an_aggregates_registered()
		{
			using (var session = _storage.BeginSession())
			{
				Should.Throw<NotSupportedException>(() => session.LoadAggregate<Toggle>(ToggleID.CreateNew()));
			}
		}

		[Fact]
		public void When_an_aggregate_is_registered()
		{
			_storage.RegisterBuilder(events => Toggle.LoadFrom(() => "", events));

			using (var session = _storage.BeginSession())
			{
				Should.Throw<KeyNotFoundException>(() => session.LoadAggregate<Toggle>(ToggleID.CreateNew()));
			}
		}
	}
}
