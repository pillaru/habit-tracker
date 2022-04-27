using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Chekkan.Login;

public static class EndpointRouteBuilderExtensions 
{
    public static void MapChekkanLogin(
            this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/Register", () => "Hello register");
    }
}
