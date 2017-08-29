using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Rest.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;

namespace Crispin.Rest
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			var container = new Container(_ =>
			{
				_.Populate(services);
				_.AddRegistry<CrispinRestRegistry>();
				_.AddRegistry<MediatrRegistry>();
			});

			return container.GetInstance<IServiceProvider>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMiddleware<ValidationMiddleware>();
			app.UseMvc();

			app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
		}
	}
}
