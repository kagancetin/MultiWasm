# MultiWasm
Multiple webassembly run together

Dotnet 7

Description:
There is 2 webassembly projects and one blazorserver project for serve webassembly pages.

How to use: 
Clone repo and run MultiWasm.Web

Build your project with Visual Studio Code.
Follow the orders;

1- Create a folder and sln.

```
mkdir MultiWasm
cd .\Multiwasm
dotnet new sln
```

2- Create serve project and add sln

```
dotnet new blazorserver -o MultiWasm.Web
dotnet sln add .\MultiWasm.Web\
```

3- configure MultiWasm.Web

a- Remove folders and files apart from these "Program.cs" , "MultiWasm.Web.csproj", "appsettings.json", "appsettings.Development.json", "obj", "Properties"
b- Add a project to 
```
cd .\MultiWasm.Web
dotnet add package Microsoft.AspNetCore.Components.WebAssembly.Server
```
c- Remove all codes in Program.js and paste that

```
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

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.MapFallbackToFile("/index.html");

app.Run();
```

d- Remove "http" node under "profiles" node that in Properties/launchSettings.json file. and change name "https" to "MultiWasm.Web". It will look like this.
```
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:4185",
      "sslPort": 44314
    }
  },
  "profiles": {
    "MultiWasm.Web": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:7113;http://localhost:5294",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

4- Time to create our base webassembly project. 

```
cd ..
dotnet new blazorwasm -o MultiWasm.Client
dotnet sln add .\MultiWasm.Client\
```

5- Configure MultiWasm.Client

a- Remove "http" node under "profiles" node that in Properties/launchSettings.json file. and change name "https" to "MultiWasm.Client". It will look like this.

```
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:37369",
      "sslPort": 44305
    }
  },
  "profiles": {
    "MultiWasm.Client": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "inspectUri": "{wsProtocol}://{url.hostname}:{url.port}/_framework/debug/ws-proxy?browser={browserInspectUri}",
      "applicationUrl": "https://localhost:7060;http://localhost:5287",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "inspectUri": "{wsProtocol}://{url.hostname}:{url.port}/_framework/debug/ws-proxy?browser={browserInspectUri}",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

6- Add MultiWasm.Client reference to MultiWasm.Web

```
dotnet add .\MultiWasm.Web\ reference .\MultiWasm.Client\ 
```

7- It's time to check that would worked
```
dotnet restore
dotnet build
dotnet run --project .\MultiWasm.Web\
```
8- Now go https://localhost:(Your port). It's work! Let's continue.
(If you see error something could gone wrong. Check the orders.)

9- It's time to create our second webassembly project and reference this.

```
dotnet new blazorwasm -o MultiWasm.Admin
dotnet sln add .\MultiWasm.Admin\
dotnet add .\MultiWasm.Web\ reference .\MultiWasm.Admin\ 
```

10- Configure MultiWasm.Admin

a- First of all we need to add below code to "MultiWasm.Admin.csproj" file
```
<StaticWebAssetBasePath>admin</StaticWebAssetBasePath>
```
It will look like this:
```
  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <StaticWebAssetBasePath>admin</StaticWebAssetBasePath>
  </PropertyGroup>
```

b- Remove "http" node under "profiles" node that in Properties/launchSettings.json file. and change name "https" to "MultiWasm.Admin".

c- And last, we change base like this '<base href="/admin/" />' in "wwwroot/index.html" 

10- Now, we need to route "/admin" path on MultiWasm.Web project. Add below code to Program.js file before "UseRouting()" method.
```
app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/admin"), appAdmin =>
{
    appAdmin.UseBlazorFrameworkFiles("/admin");
    appAdmin.UseRouting();
    appAdmin.UseEndpoints(endpoints =>
    {
        endpoints.MapFallbackToFile("/admin/{*path:nonfile}", "/admin/index.html");
    });
});
```

7- It's finish. Let's check
```
dotnet restore
dotnet build
dotnet run --project .\MultiWasm.Web\
```

and go https://localhost:(Your port)/admin

