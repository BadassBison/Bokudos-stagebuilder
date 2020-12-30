using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using StageBuilder.Database;
using StageBuilder.Profiles;
using StageBuilder.Services;

namespace StageBuilder
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public virtual void ConfigureServices(IServiceCollection services)
    {
      var connection = Configuration.GetConnectionString("DockerDatabaseConnection");

      services.AddDbContext<StageBuilderDbContext>(
          options => options.UseSqlServer(connection));

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddTransient<IStageService, StageService>();

      services.AddAutoMapper(typeof(StageProfile));

      services.AddControllers();

      services.AddCors(options =>
      {
        options.AddDefaultPolicy(
          builder =>
          {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
          });
      });

      services.AddMvc();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Version = "v1",
          Title = "StageBuilder",
          Description = "A API for saving and fetching game stage data",
          TermsOfService = new Uri("https://example.com/terms"),
          Contact = new OpenApiContact
          {
            Name = "Shawn Scott",
            Email = "shawn.scott.xd@gmail.com",
            Url = new Uri("https://github.com/BadassBison"),
          },
          License = new OpenApiLicense
          {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license"),
          }
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
      });
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      app.UseCors();

      app.UseSwagger();

      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
      });

      SetupDb.SetupConfig(app);
    }
  }
}
