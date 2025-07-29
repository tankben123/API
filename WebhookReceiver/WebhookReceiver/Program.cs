using WebhookReceiver.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

app.MapControllers();
app.MapHub<SheetChangeEventHub>("/hubs/sheetChange");

app.Run();
