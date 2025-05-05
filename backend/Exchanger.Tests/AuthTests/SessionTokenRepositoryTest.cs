namespace Exchanger.Tests.AuthTests;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exchanger.API.Data;
using Exchanger.API.Entities;
using Exchanger.API.Repositories;

public class SessionTokenRepositoryTest
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
            new SessionToken { Id = Guid.NewGuid(), UserId = userId, CreatedAt = 
            DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(14), DeviceType = 
            "Comp", IpAdress = "myAdress", IsRevoked = false },
            new SessionToken { Id = Guid.NewGuid(), UserId = userId, CreatedAt = DateTime.UtcNow, 
                ExpiresAt = DateTime.UtcNow.AddDays(14), DeviceType = "Comp", IpAdress = "myAdress", 
                IsRevoked = true },
            new SessionToken { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, 
                ExpiresAt = DateTime.UtcNow.AddDays(14), DeviceType = "Comp", IpAdress = "myAdress",
                IsRevoked = false }
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