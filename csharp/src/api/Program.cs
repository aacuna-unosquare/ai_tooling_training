using api.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<IIdentifierGenerator, IdentifierGenerator>();

var app = builder.Build();
app.MapControllers();
app.Run();
