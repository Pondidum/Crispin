using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileSystem;
using Newtonsoft.Json;

namespace Crispin.Infrastructure.Statistics
{
	public class FileSystemStatisticsStore : IStatisticsStore
	{
		private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
		{
			Formatting = Formatting.None,
			TypeNameHandling = TypeNameHandling.Objects
		};

		private readonly IFileSystem _fileSystem;
		private readonly string _root;
		private readonly List<Func<DateTime, IStat, Task>> _projections;

		public FileSystemStatisticsStore(IFileSystem fileSystem, string root)
		{
			_fileSystem = fileSystem;
			_root = root;

			_projections = new List<Func<DateTime, IStat, Task>>();
		}

		public async Task Append(DateTime timestamp, IStat stat)
		{
			// append to stats.raw.json
			var statsLogPath = Path.Combine(_root, "stats.log.json");
			await _fileSystem.AppendFileLines(
				statsLogPath,
				JsonConvert.SerializeObject(stat, JsonSerializerSettings));

			await Task.WhenAll(_projections.Select(p => p(timestamp, stat)));
		}
	}
}
