var builder = WebApplication.CreateBuilder(args);


var portConfig = bl.ConHelper.MyPort();

builder.WebHost.UseUrls($"http://localhost:{portConfig.Port}");

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
