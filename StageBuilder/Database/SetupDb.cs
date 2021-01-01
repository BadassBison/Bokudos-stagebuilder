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
              UserId = 1,
              GameId = 1,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now,
              Regions = new List<RegionEntity>
              {
                new RegionEntity
                {
                  Row = 0,
                  Column = 0,
                  Data = "1,1,1,1n2,2,2,2n3,3,3,3"
                }
              }
            },
            new StageEntity
            {
              Name = "StageTwo",
              UserId = 1,
              GameId = 1,
              CreatedDate = DateTime.Now,
              LastUpdatedDate = DateTime.Now,
              Regions = new List<RegionEntity>
              {
                new RegionEntity
                {
                  Row = 0,
                  Column = 0,
                  Data = "1,1,1,1n2,2,2,2n3,3,3,3"
                }
              }
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
