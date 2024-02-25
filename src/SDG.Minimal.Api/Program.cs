using SDG.Minimal.Api;
using SDG.Minimal.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();
builder.RegisterWebHost();

var app = builder.Build();

app.RegisterMiddlewares();

app.MapTodoEndpoint();

app.Run();
