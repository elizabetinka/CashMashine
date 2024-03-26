using System;
using System.Threading.Tasks;
using Itmo.ObjectOrientedProgramming.Lab3.Tests;
using Lab5.Tests.Data;
using Models;
using Moq;
using Services;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

#pragma warning disable CA2007
public class IncreaseTests
{
    [Theory]
    [ClassData(typeof(IncreaseTestsData))]
    public async Task IncreaseMoneyTest(User user, int money, int ans)
    {
        // Arrang
        user = user ?? throw new ArgumentNullException(nameof(user));
        var mock = new Mock<IPersonRepository>();
        IUserService userService = new UserService(mock.Object);
        mock.Setup(c => c.FindById(It.IsAny<int>())).ReturnsAsync((int i) => user);

        // Act
        bool res = await userService.TryAddCash(money, user.Id);

        // Assert
        Assert.True(res);
        Assert.Equal(user.Balance, ans);
    }
}