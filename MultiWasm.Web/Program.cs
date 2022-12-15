// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Web;

// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddRazorPages();
// builder.Services.AddServerSideBlazor();

// var app = builder.Build();

// if (!app.Environment.IsDevelopment())
// {
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection();

// app.UseStaticFiles();

// app.UseRouting();

// app.MapBlazorHub();
// app.MapFallbackToPage("/_Host");

// app.Run();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseBlazorFrameworkFiles("");

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/admin"), appAdmin =>
{
    appAdmin.UseBlazorFrameworkFiles("/admin");
    appAdmin.UseRouting();
    appAdmin.UseEndpoints(endpoints =>
    {
        endpoints.MapFallbackToFile("/admin/{*path:nonfile}", "/admin/index.html");
    });
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.MapFallbackToFile("/index.html");

app.Run();
