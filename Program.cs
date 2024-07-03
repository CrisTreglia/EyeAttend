using EyeAttend.Data;
using EyeAttend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Services Configuration
builder.Services.AddControllersWithViews(); // For MVC views

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add support for API Controllers
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Database Context Configuration
builder.Services.AddDbContext<EyeAttendDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Service Registration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "EyeAttend API",
        Description = "An API for EyeAttend application",
        // Add more details as needed (terms of service, contact, etc.)
    });

    // (Optional) If you use XML comments, add this to include them in the documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Middleware Pipeline Configuration

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // MVC error page
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EyeAttend API v1");
        c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Configure endpoints for both MVC and API Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers(); // API routes - this is crucial for Web API controllers

app.Run();
