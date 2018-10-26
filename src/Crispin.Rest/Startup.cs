using System;
using System.Net;
using System.Threading.Tasks;
using Crispin.Infrastructure;
using Crispin.Rest.Configuration;
using Crispin.Rest.Infrastructure;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Crispin.Rest
{
	public class Startup
	{
		public void ConfigureContainer(ServiceRegistry services)
		{
			services
				.AddMvc(options =>
				{
					options.Filters.Add<JsonNotFoundActionFilter>();
					options.Filters.Add<ValidationExceptionFilter>();
					options.Filters.Add<NotFoundExceptionFilter>();

					options.ModelBinderProviders.Insert(0, new ToggleLocatorBinder());
					options.ModelBinderProviders.Insert(1, new DomainIDBinder());
				}).AddJsonOptions(options =>
				{
					options.SerializerSettings.Converters.Add(new StringEnumConverter  { CamelCaseText = true });
				});


			services.Scan(_ =>
			{
				_.TheCallingAssembly();
				_.AssemblyContainingType<Toggle>();
				_.LookForRegistries();
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				routes.MapSpaFallbackRoute(
					name: "spa-fallback",
					defaults: new { controller = "Home", action = "Index" });
			});
		}
	}
}
