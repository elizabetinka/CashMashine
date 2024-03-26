using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http.Features;
using Models;
using Services;

namespace WebApplication6;
public class AuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
{
    private IPersonRepository _service;
    private IErrorDescriber _errorDescriber;

    public AuthorizationMiddleware(IPersonRepository service, IErrorDescriber errorDescriber)
    {
        _service = service;
        _errorDescriber = errorDescriber;
    }

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        next = next ?? throw new ArgumentNullException(nameof(next));
        context = context ?? throw new ArgumentNullException(nameof(context));
        policy = policy ?? throw new ArgumentNullException(nameof(policy));

        KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> authHeader = context.Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization", StringComparison.Ordinal));

        if (authHeader.Key is null)
        {
            _errorDescriber.HaventBeenAuthorizated(context);
            return;
        }

        var value = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);

        IPerson? person = null;
        string? username = string.Empty;
        string? password = string.Empty;
        if (value.Parameter != null)
        {
            string[] strs = Encoding.UTF8.GetString(Convert.FromBase64String(value.Parameter)).Split(':');
            username = strs.FirstOrDefault();
            password = strs.LastOrDefault();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _errorDescriber.HaventBeenWrittenData(context);
                IHttpResponseFeature? feature = context.Features.Get<IHttpResponseFeature>();
                if (feature is not null)
                {
                    feature.ReasonPhrase = "HaventBeenWrittenData";
                }

                AuthorizationFailure.Failed(policy.Requirements);
                return;
            }

            person = await _service.FindByUsername(username);
        }

        if (person is null)
        {
            _errorDescriber.PersonNotFounded(context);
            return;
        }

        if (!person.Password.Equals(password, StringComparison.Ordinal))
        {
            _errorDescriber.WrongPassword(context);
            return;
        }

        foreach (IAuthorizationRequirement requirement in policy.Requirements)
        {
            if (requirement is not RolesAuthorizationRequirement rolesAuthorizationRequirement)
            {
                continue;
            }

            if (rolesAuthorizationRequirement.AllowedRoles.Contains(person.Role))
            {
                continue;
            }

            _errorDescriber.NoAccessToData(context);
            return;
        }

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, person.Role),
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        await next(context);
    }
}