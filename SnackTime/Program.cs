using Microsoft.AspNetCore.Authentication.Cookies;
using SnackTime.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Fix infinite cycle on n:n relations
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System
        .Text
        .Json
        .Serialization
        .ReferenceHandler
        .IgnoreCycles;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect to login if not authenticated
        options.LogoutPath = "/Account/Logout"; // Redirect to logout action

        // Configure the expiration time for sessions
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

        // Sliding expiration makes it so the expiration time of a session gets
        // pushed back when the user is active
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(
    (config) =>
    {
        config.SwaggerEndpoint("v1/swagger.json", "Event Planner");
        config.RoutePrefix = "swagger";
    }
);

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();