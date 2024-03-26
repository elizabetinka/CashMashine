namespace WebApplication6;

public class ErrorDescriber : IErrorDescriber
{
    public void HaventBeenWrittenData(
        HttpContext context)
    {
        context = context ?? throw new ArgumentNullException(nameof(context));
        if (context.Response.HasStarted) return;
        context.Response.StatusCode = 404;
        context.Response.Headers.Append("error", "Haven't Been Written Data");
        context.Response.WriteAsync("HaventBeenWrittenData");
    }

    public void HaventBeenAuthorizated(
        HttpContext context)
    {
        context = context ?? throw new ArgumentNullException(nameof(context));
        if (context.Response.HasStarted) return;
        context.Response.StatusCode = 401;
        context.Response.Headers.Append("error", "Haven t Been Authorizated"); // неаутентифицированный
        context.Response.WriteAsync("Haven t Been Authorizated");
    }

    public void PersonNotFounded(
        HttpContext context)
    {
        context = context ?? throw new ArgumentNullException(nameof(context));
        if (context.Response.HasStarted) return;
        context.Response.StatusCode = 404;
        context.Response.Headers.Append("error", "Person Not Founded");
        context.Response.WriteAsync("PersonNotFounded");
    }

    public void WrongPassword(
        HttpContext context)
    {
        context = context ?? throw new ArgumentNullException(nameof(context));
        if (context.Response.HasStarted) return;
        context.Response.StatusCode = 403;
        context.Response.Headers.Append("error", "Wrong Password");
        context.Response.WriteAsync("Wrong Password");
    }

    public void NoAccessToData(
        HttpContext context)
    {
        context = context ?? throw new ArgumentNullException(nameof(context));
        if (context.Response.HasStarted) return;
        context.Response.StatusCode = 403;
        context.Response.Headers.Append("error", "No Access To Data"); // неавторизованный
        context.Response.WriteAsync("NoAccessToData");
    }
}