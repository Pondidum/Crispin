using Crispin.Rest.Configuration;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Crispin.Rest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseLamar()
				.UseStartup<Startup>()
				.ConfigureServices(InjectStrongConfiguration)
				.ConfigureLogging((hosting, logging) =>
				{
					logging.AddConfiguration(hosting.Configuration.GetSection("Logging"));
					logging.AddDebug();
				})
				.Build();

		private static void InjectStrongConfiguration(WebHostBuilderContext host, IServiceCollection services)
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true)
				.Build();

			var config = builder.Get<CrispinConfiguration>() ?? new CrispinConfiguration();

			services.AddSingleton(StorageBuilder.Build(config));
			services.AddSingleton(config);
		}
	}
}
