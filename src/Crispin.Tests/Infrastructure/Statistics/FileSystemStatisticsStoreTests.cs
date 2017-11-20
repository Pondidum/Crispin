using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Infrastructure.Statistics;
using FileSystem;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Statistics
{
	public class FileSystemStatisticsStoreTests
	{
		private const string StatsRoot = "./stats";

		private readonly InMemoryFileSystem _fs;
		private readonly FileSystemStatisticsStore _stats;

		public FileSystemStatisticsStoreTests()
		{
			_fs = new InMemoryFileSystem();
			_fs.CreateDirectory(StatsRoot).Wait();
			_stats = new FileSystemStatisticsStore(_fs, StatsRoot);
		}

		[Fact]
		public async Task When_saving_a_statistic()
		{
			var stat = new ToggleRead(ToggleID.CreateNew());
			var now = DateTime.UtcNow;

			await _stats.Append(now, stat);

			var lines = await _fs.ReadFileLines(Path.Combine(StatsRoot, "stats.log.json"));

			lines.Single().ShouldNotBeNull();
		}
	}
}
