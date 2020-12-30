using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;

using StageBuilder.Database;
using StageBuilder.Services;
using StageBuilder.Models;
using StageBuilder.Dtos;

namespace StageBuilder.Tests
{
  public class StageServiceTests
  {
    string Entities = "stages";

    [Fact]
    public async Task GetAllStages_WhenThreeAreAdded_ReturnsThree()
    {
      // Arrange
      var connectionStringBuild =
          new SqliteConnectionStringBuilder { DataSource = ":memory:" };
      var connection = new SqliteConnection(connectionStringBuild.ToString());

      var options = new DbContextOptionsBuilder<StageBuilderDbContext>()
          .UseSqlite(connection)
          .Options;

      using (var context = new StageBuilderDbContext(options))
      {
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        context.AddRange(new List<StageEntity>
        {
          new StageEntity()
          {
            Name = "Test stage 1",
            Data = "1,2,3,4,5n6,7,8,9,0",
            UserId = 1,
            GameId = 1,
            CreatedDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now
          },
          new StageEntity()
          {
            Name = "Test stage 2",
            Data = "1,2,3,4,5n6,7,8,9,0",
            UserId = 1,
            GameId = 1,
            CreatedDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now
          },
          new StageEntity()
          {
            Name = "Test stage 3",
            Data = "1,2,3,4,5n6,7,8,9,0",
            UserId = 1,
            GameId = 1,
            CreatedDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now
          }
        }.ToArray());
        context.SaveChanges();
      }

      using (var context = new StageBuilderDbContext(options))
      {
        var service = new StageService(context);

        // Act
        var stages = await service.GetAllStagesAsync();

        // Assertion
        var expectedCount = 3;
        var msg = $"There are {expectedCount} {Entities} in the database";
        stages.Should().HaveCount(expectedCount, because: msg);
      }
    }

    [Fact]
    public async Task AddStageAsync_AddsStage_WhenSuccessful()
    {
      // Arrange
      var connectionStringBuild =
          new SqliteConnectionStringBuilder { DataSource = ":memory:" };
      var connection = new SqliteConnection(connectionStringBuild.ToString());

      var options = new DbContextOptionsBuilder<StageBuilderDbContext>()
          .UseSqlite(connection)
          .Options;

      using (var context = new StageBuilderDbContext(options))
      {
        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();

        var stage = new StageEntity()
        {
          Name = "Test stage 1",
          Data = "1,2,3,4,5n6,7,8,9,0",
          UserId = 1,
          GameId = 1,
          CreatedDate = DateTime.Now,
          LastUpdatedDate = DateTime.Now
        };
        var service = new StageService(context);

        // Act 1
        var entity = await service.AddStageAsync(stage);

        // Assertion 1
        var expected = "Test stage 1";
        var msg = $"Stage {entity.Name} was returned from add method";
        entity.Name.Should().Be(expected, because: msg);
      }

      using (var context = new StageBuilderDbContext(options))
      {
        var stages = context.Stages;

        // Act 2 - Check if item is is DB
        var id = 1;
        var entity = stages.First(p => p.StageId == id);

        // Assertion 2
        var expected = "Test stage 1";
        var msg = $"Stage {entity.Name} exists in database";
        entity.Name.Should().Be(expected, because: msg);
      }
    }

    [Fact]
    public async Task UpdateStage_UpdatesAStageInDB_ReturnsObject()
    {
      // Arrange
      var connectionStringBuild =
        new SqliteConnectionStringBuilder { DataSource = ":memory:" };
      var connection = new SqliteConnection(connectionStringBuild.ToString());

      var options = new DbContextOptionsBuilder<StageBuilderDbContext>()
        .UseSqlite(connection)
        .Options;

      var entity = new StageEntity()
      {
        Name = "Test stage 1",
        Data = "1,2,3,4,5n6,7,8,9,0",
        UserId = 1,
        GameId = 1,
        CreatedDate = DateTime.Now,
        LastUpdatedDate = DateTime.Now
      };

      using (var context = new StageBuilderDbContext(options))
      {
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        context.Stages.Add(entity);
        context.SaveChanges();
      }

      using (var context = new StageBuilderDbContext(options))
      {
        var updatedStage = new Stage()
        {
          Name = "Updated stage 1",
          Data = "6,7,8,9,0n1,2,3,4,5",
          UserId = 1,
          GameId = 1,
        };

        var service = new StageService(context);

        // Act 1
        var updatedEntity = await service.UpdateStageAsync(entity, updatedStage);

        // Assertion 1
        var expectedName = "Updated stage 1";
        var expectedData = "6,7,8,9,0n1,2,3,4,5";
        var msg = $"Stage {updatedEntity.Name} has an updated";
        using (new AssertionScope())
        {
          updatedEntity.Name.Should().Be(expectedName, because: msg);
          updatedEntity.Data.Should().Be(expectedData, because: msg);
        }
      }
    }

    [Fact]
    public async Task RemoveStage_RemovesStage_ReturnsObject()
    {
      // Arrange
      var connectionStringBuild =
        new SqliteConnectionStringBuilder { DataSource = ":memory:" };
      var connection = new SqliteConnection(connectionStringBuild.ToString());

      var options = new DbContextOptionsBuilder<StageBuilderDbContext>()
          .UseSqlite(connection)
          .Options;

      using (var context = new StageBuilderDbContext(options))
      {
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        context.Stages.Add(new StageEntity()
        {
          Name = "Test stage 1",
          Data = "1,2,3,4,5n6,7,8,9,0",
          UserId = 1,
          GameId = 1,
          CreatedDate = DateTime.Now,
          LastUpdatedDate = DateTime.Now
        });
        context.SaveChanges();
      }

      using (var context = new StageBuilderDbContext(options))
      {
        var service = new StageService(context);
        string name = "Test stage 1";
        var stage = context.Stages.First(p => p.Name == name);

        // Act
        var entity = await service.RemoveStageAsync(stage);

        // Assertion 1
        var expectedName = "Test stage 1";
        var msg = $"Stage {entity.Name} was returned from the remove method";
        entity.Name.Should().Be(expectedName, because: msg);
      }

      using (var context = new StageBuilderDbContext(options))
      {
        // Assertion 2
        var expected = 0;
        var msg = $"There are no entities in the database";
        context.Stages.Should().HaveCount(expected, because: msg);
      }
    }
  }
}
