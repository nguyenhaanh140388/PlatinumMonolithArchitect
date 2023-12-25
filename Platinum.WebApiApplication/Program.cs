using Microsoft.AspNetCore.Mvc.Infrastructure;
using Platinum.Core.Utils;
using Platinum.WebApiApplication.Extensions;
using Platinum.WebApiApplication.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
.AddEnvironmentVariables()
.Build();


//builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//******** Filter ********//
builder.Services.AddFilterExtension(configuration);

builder.Services.AddResponseCaching();
builder.Services.AddResponseCompression();

//******** Json ********//.
builder.Services.AddJsonConfiguration();

//******** Serial log ********//.
// Add services to the container.
var connStr = builder.Configuration["ConnectionStrings:Seriallog"];
builder.Services.AddSerialLog(configuration);

//******** SignalR ********//
//builder.Services.AddSignalR();

//******** AutoMapper ********//
//Assembly.Load("AngularMongo20230318.Infrastructure");
//Assembly.Load("AngularMongo20230318.Core");
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//******** IdentityAndAuthentication ********//
builder.Services.RegisterIdentityAndAuthentication(configuration);

//******** Cors ********//
builder.Services.AddCorsExtension(configuration);



//******** SharedServices ********//
//builder.Services.RegisterSharedServices(configuration);

//******** Autofac ********//
builder.Host.AddAutofac(builder.Services, configuration);

var app = builder.Build();

ApplicationHttpContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>(), app.Services.GetRequiredService<IActionContextAccessor>());

app.UseCors("DefaultCor");

//******** CultureInfo ********//
app.UseCultureInfo();


// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();
// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
