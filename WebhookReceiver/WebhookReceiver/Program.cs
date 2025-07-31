using WebhookReceiver.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ Controller và SignalR
builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
{
    // Giữ kết nối chủ động hơn với ping mỗi 10s
    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

// Định tuyến HTTP
app.UseRouting();

// Ánh xạ endpoint cho Controller và SignalR
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers(); // Webhook controller
    _ = endpoints.MapHub<SheetChangeEventHub>("/hubs/sheetChange"); // SignalR Hub
    _ = endpoints.MapGet("/ping", () => Results.Ok("Still alive")); // Ping để giữ app không sleep
});

// Chạy ứng dụng
app.Run();
