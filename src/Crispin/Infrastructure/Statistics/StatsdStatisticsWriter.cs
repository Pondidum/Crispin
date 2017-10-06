using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers.GetAll;
using Crispin.Handlers.GetSingle;
using Crispin.Handlers.UpdateState;
using StatsdClient;

namespace Crispin.Infrastructure.Statistics
{
	public class StatsdStatisticsWriter : IStatisticsWriter
	{
		private static readonly Dictionary<Type, Action<object>> Mapper;

		private static void Add<T>(Action<T> map) => Mapper[typeof(T)] = metric => map((T)metric);

		static StatsdStatisticsWriter()
		{
			Mapper = new Dictionary<Type, Action<object>>();

			Add<ToggleRead>(m => Metrics.Counter($"toggle.{m.ToggleID}.read"));
			Add<MultipleTogglesRead>(m => m.Toggles.Each(id => Metrics.Counter($"toggle.{id}.read")));
			Add<ToggleDefaultStateChange>(m => Metrics.Counter($"toggle.{m.ToggleID}.state.default.{m.State}"));
			Add<ToggleUserStateChange>(m => Metrics.Counter($"toggle.{m.ToggleID}.state.users.{m.UserID}.{m.State.Render()}"));
			Add<ToggleGroupStateChange>(m => Metrics.Counter($"toggle.{m.ToggleID}.state.groups.{m.GroupID}.{m.State.Render()}"));
		}

		public StatsdStatisticsWriter()
		{
			Metrics.Configure(new MetricsConfig
			{
				Prefix = "crispin",
				StatsdServerName = "localhost"
			});
		}

		public async Task WriteCount(IStat stat)
		{
			if (Mapper.TryGetValue(stat.GetType(), out var handler))
				await Task.Run(() => handler(stat));
		}
	}
}
