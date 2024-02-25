using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SDG.Minimal.Api.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SDG.Minimal.Api
{
	public static class Configuration
	{
		public static void RegisterServices(this WebApplicationBuilder builder)
		{
			builder.Services
					.AddEndpointsApiExplorer()
					.AddSwaggerGen(ac =>
					{
						ac.EnableAnnotations();

						ac.SwaggerDoc("v1", new OpenApiInfo
						{
							Version = "v1",
							Title = "SDG's Minimal",
							Description = $".NET 8 Web API",
							Contact = new OpenApiContact
							{
								Name = "Donggu Seo",
								//Url = new Uri(),
							}
						});

						// Set the comments path for the Swagger JSON and UI.
						var xmlFileList = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();

						foreach (string file in xmlFileList)
						{
							ac.IncludeXmlComments(file);
						}
					})
					.AddDbContext<DBTodo>(opt => opt.UseInMemoryDatabase("TodoList"));
		}

		public static void RegisterWebHost(this WebApplicationBuilder builder)
		{
			builder.WebHost
				.ConfigureKestrel(options =>
				{
					options.AddServerHeader = false;
				});
		}

		public static void RegisterMiddlewares(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger()
				   .UseSwaggerUI(c =>
				   {
					   // Swagger json syntax highlight.
					   c.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
					   {
						   ["activated"] = true
					   };

					   // Swagger endpoint.
					   c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

					   c.RoutePrefix = "swagger";
				   });
			}
			
			app.UseHttpsRedirection();
		}
	}
}
