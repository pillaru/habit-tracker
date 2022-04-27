using Chekkan.Login;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapChekkanLogin();

app.Run();
