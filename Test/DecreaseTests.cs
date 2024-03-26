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
public class DecreaseTests
{
    [Theory]
    [ClassData(typeof(DecreaseTestsGood))]
    public async Task GoodDecreaseMoneyTest(IPerson user, int money, int ans)
    {
        // Arrang
        user = user ?? throw new ArgumentNullException(nameof(user));
        var mock = new Mock<IPersonRepository>();
        IUserService userService = new UserService(mock.Object);
        mock.Setup(c => c.FindById(It.IsAny<int>())).ReturnsAsync((int i) => user);

        // Act
        bool res = await userService.TryTakeCash(money, user.Id);

        // Assert
        Assert.True(res);
        var user2 = (User)user;
        Assert.Equal(user2.Balance, ans);
    }

    [Theory]
    [ClassData(typeof(DecreaseTestsBad))]
    public async Task BadDecreaseMoneyTest(IPerson user, int money, int ans = 0)
    {
        // Arrang
        user = user ?? throw new ArgumentNullException(nameof(user));
        var mock = new Mock<IPersonRepository>();
        IUserService userService = new UserService(mock.Object);
        mock.Setup(c => c.FindById(It.IsAny<int>())).ReturnsAsync((int i) => user);

        // Act
        bool res = await userService.TryTakeCash(money, user.Id);

        // Assert
        Assert.False(res);
        if (user is User u)
        {
            Assert.Equal(u.Balance, ans);
        }
    }
}