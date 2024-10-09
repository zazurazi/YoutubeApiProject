using YouTubeApiProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register YouTubeApiService
builder.Services.AddScoped<YouTubeApiService>();

// Register HttpClient for API requests
builder.Services.AddHttpClient();  // This line registers HttpClient

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Set the default route to YouTubeController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=YouTube}/{action=Search}/{id?}");

app.Run();
