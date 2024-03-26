using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication6.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("[controller]")]
[SwaggerTag("Functions for administrators")]
public class AdminController : ControllerBase
{
    private IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost(Name = "Create expense")]
    [SwaggerOperation("Create expense")]
    public async Task<string> CreateExpense(string username, string password)
    {
        if (await _adminService.TryAddUser(username, password))
        {
            return string.Format(CultureInfo.InvariantCulture, "Рады, что вы с нами,{0} ", username);
        }

        return string.Format(CultureInfo.InvariantCulture, "Не удалось открыть счет на имя {0}, попробуйте поменять username", username);
    }

    [HttpPut(Name = "Add admin")]
    [SwaggerOperation("Add admin")]
    public async Task<string> AddAdmin(string username)
    {
        if (await _adminService.TryAddAdmin(username))
        {
            return string.Format(CultureInfo.InvariantCulture, "Рады, что вы с нами,{0} ", username);
        }

        return string.Format(CultureInfo.InvariantCulture, "Не удалось добавить админа с именем {0}, попробуйте поменять username", username);
    }

    [HttpDelete(Name = "Change password")]
    [SwaggerOperation("Change password")]
    public async Task<string> ChangePassword(string currentPassword, string newPassword)
    {
        if (await _adminService.TryChangeAdminPassword(currentPassword, newPassword))
        {
            return "Пароль  успешно изменен";
        }

        return "Введен неверный текущий пароль ";
    }
}