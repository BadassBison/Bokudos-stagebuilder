using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StageBuilder.Models;

namespace StageBuilder.Database
{
  public static class SetupDb
  {
    public static void SetupConfig(IApplicationBuilder app)
    {
      using (var serviceScope = app.ApplicationServices.CreateScope())
      {
        seedDb(serviceScope.ServiceProvider.GetService<StageBuilderDbContext>());
      }
    }

    public static void seedDb(StageBuilderDbContext context)
    {
      System.Console.WriteLine("Appling Migrations...");

      context.Database.Migrate();

      if (!context.Stages.Any())
      {
        System.Console.WriteLine("Seeding data...");

        context.Stages.AddRange(
          new List<StageEntity>()
          {
            new StageEntity
            {
              Name = "StageOne",
              Data = "1,2,3,4,5n6,7,8,9,0",
              UserId = 1,
              GameId = 1,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            },
            new StageEntity
            {
              Name = "StageTwo",
              Data = "6,7,8,9,0n1,2,3,4,5",
              UserId = 1,
              GameId = 1,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now
            },
          }.ToArray()
        );
        context.SaveChanges();
      }
      else
      {
        System.Console.WriteLine("Already had data, not seeding data...");
      }
    }
  }
}
