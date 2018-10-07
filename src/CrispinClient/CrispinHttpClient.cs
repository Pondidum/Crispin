using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CrispinClient
{
	public class CrispinHttpClient : ICrispinClient, IDisposable
	{
		private readonly Uri _uri;
		private readonly HttpClient _client;

		public CrispinHttpClient(Uri uri, HttpClient httpClient = null)
		{
			_client = httpClient ?? new HttpClient();
			_uri = uri;
		}

		public async Task<IEnumerable<Toggle>> GetAllToggles()
		{
			var url = new Uri(_uri, "toggles");
			var json = await _client.GetStringAsync(url);

			return JsonConvert.DeserializeObject<IEnumerable<Toggle>>(json);
		}

		public async Task SendStatistics(IEnumerable<Statistic> statistics)
		{
			var url = new Uri(_uri, "stats");
			var json = JsonConvert.SerializeObject(statistics);

			var response = await _client.PostAsync(
				url,
				new StringContent(json, Encoding.UTF8, "application/json"));

			response.EnsureSuccessStatusCode();
		}

		public void Dispose()
		{
			_client.Dispose();
		}
	}
}
