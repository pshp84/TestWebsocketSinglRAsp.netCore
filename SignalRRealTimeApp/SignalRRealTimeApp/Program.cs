using SignalRRealTimeApp.Hubs;
using SignalRRealTimeApp.Models;
using SignalRRealTimeApp.Setup;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
});
DependenciesConfig.ConfigureDependencies(builder.Services);
var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseWebSockets();
app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

// Map endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.Run();
