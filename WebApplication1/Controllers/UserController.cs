using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication6.Controllers;

[Authorize(Roles = "user")]
[ApiController]
[Route("[controller]")]
[SwaggerTag("Functions for users")]
public class UserController : ControllerBase
{
    private IPersonRepository _personService;
    private IUserService _userService;

    public UserController(IPersonRepository personService, IUserService userService)
    {
        _personService = personService;
        _userService = userService;
    }

    [HttpPost(Name = "See operations history")]
    [SwaggerOperation("See operations history (show the last 10 operations. If you want to change this value, enter it in the field below)")]
    public async Task<string> OperationsHistory(int operationsCount = 10)
    {
        if (HttpContext.User.Identity is null || string.IsNullOrEmpty(HttpContext.User.Identity.Name)) return "вы не авторизованы";
        IPerson? person = await _personService.FindByUsername(HttpContext.User.Identity.Name);
        if (person is null)
        {
            return "пользователя не существует";
        }

        if (person is User p)
        {
            return string.Join('\n', await _userService.GetOperationsHistory(p.Id, operationsCount));
        }

        return "вы админ, у вас нет денег";
    }

    [HttpPut(Name = "Put cash")]
    [SwaggerOperation("Put cash")]
    public async Task<string> PutCash(int money)
    {
        if (HttpContext.User.Identity is null || string.IsNullOrEmpty(HttpContext.User.Identity.Name)) return "вы не авторизованы";
        IPerson? person = await _personService.FindByUsername(HttpContext.User.Identity.Name);
        if (person is null)
        {
            return "пользователя не существует";
        }

        if (person is User p)
        {
            if (await _userService.TryAddCash(money,  p.Id))
            {
                return string.Format(CultureInfo.InvariantCulture, "Операция прошла успешно. Ваш баланс: {0} ", p.Balance + money);
            }

            return string.Format(CultureInfo.InvariantCulture, "Не удалось положить требуемую сумму");
        }

        return "вы админ, у вас нет денег";
    }

    [HttpDelete(Name = "Take cash")]
    [SwaggerOperation("Take cash")]
    public async Task<string> TakeCash(int countOfMoney)
    {
        if (HttpContext.User.Identity is null || string.IsNullOrEmpty(HttpContext.User.Identity.Name)) return "вы не авторизованы";
        IPerson? person = await _personService.FindByUsername(HttpContext.User.Identity.Name);
        if (person is null)
        {
            return "пользователя не существует";
        }

        if (person is User p)
        {
            if (p.Balance >= countOfMoney && await _userService.TryTakeCash(countOfMoney,  p.Id))
            {
                return string.Format(CultureInfo.InvariantCulture, "Операция прошла успешно. Ваш баланс: {0} ", p.Balance - countOfMoney);
            }

            return string.Format(CultureInfo.InvariantCulture, "Не удалось снять требуемую сумму");
        }

        return "вы админ, у вас нет денег";
    }

    [HttpGet(Name = "Get My Balance")]
    [SwaggerOperation("See My Balance")]
    public async Task<string> SeeBalance()
    {
        if (HttpContext.User.Identity is null || string.IsNullOrEmpty(HttpContext.User.Identity.Name)) return "вы не авторизованы";
        IPerson? person = await _personService.FindByUsername(HttpContext.User.Identity.Name);
        if (person is null)
        {
            return "пользователя не существует";
        }

        if (person is User p)
        {
            return string.Format(CultureInfo.InvariantCulture, "Ваш баланс: {0} ", p.Balance);
        }

        return "вы админ, у вас нет денег";

        // return string.Format(CultureInfo.InvariantCulture, "id: {1} имя: {0} роль: {2} ", person.Username, person.Id, person.Role);
    }
}