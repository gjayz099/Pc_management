using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


var portConfig = bl.ConHelper.MyPort();

builder.WebHost.UseUrls($"http://localhost:{portConfig.Port}");

builder.Services.AddSession(options =>
{
    // Sets the name of the session cookie.
    options.Cookie.Name = "_PCPMS";

    // Sets the timeout period for the session. 
    // The session will expire after 30 minutes of inactivity.
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    // Marks the session cookie as HTTP only. 
    // This helps mitigate the risk of client side script accessing the protected cookie data.
    options.Cookie.HttpOnly = true;

    // Marks the session cookie as essential. 
    // This ensures the cookie is still stored even if the user has not consented to non-essential cookies.
    options.Cookie.IsEssential = true;
});



// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"C:\Users\Glaiza\Documents\PCPMS\img"),
    RequestPath = "/img"
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

app.Run();
