namespace Exchanger.Tests;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exchanger.API.Data;
using Exchanger.API.Entities;
using Exchanger.API.Repository;

public class SessionTokenRepositoryTest
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // унікальна база для кожного тесту
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Should_Return_Only_Unexpired_Tokens()
    {
        var userId = Guid.NewGuid();

        using var context = CreateContext();
        context.SessionTokens.AddRange(new List<SessionToken>
        {
            new SessionToken { Id = Guid.NewGuid(), UserId = userId, IsRevoked = false },
            new SessionToken { Id = Guid.NewGuid(), UserId = userId, IsRevoked = true },
            new SessionToken { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), IsRevoked = false }
        });

        await context.SaveChangesAsync();

        var repository = new SessionTokenRepository(context);

        var result = await repository.GetUnexpiredTokensByUserIdAsync(userId);
        Assert.Single(result);
        Assert.All(result, token =>
        {
            Assert.Equal(userId, token.UserId);
            Assert.False(token.IsRevoked);
        });
    }
}