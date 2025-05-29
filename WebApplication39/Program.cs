var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddResponseCaching();

var app = builder.Build();

app.UseResponseCaching();

app.MapControllerRoute(
    name:"default",
    pattern:"{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
