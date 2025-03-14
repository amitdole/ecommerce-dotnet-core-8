using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


//ocelot configuration
builder.Host.ConfigureAppConfiguration((env, config) =>
{
    config.AddJsonFile($"ocelot.{env.HostingEnvironment.EnvironmentName}.json", true, true);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOcelot();

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();


//Identity Server
//var authScheme = "EShoppingGatewayAuthScheme";

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(authScheme, options => {
//        options.Authority = "https://localhost:9009";
//        options.Audience = "EShoppingGateway";
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("CorsPolicy");  

//app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    var endpointConventionBuilder = endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello Ocelot!");
    });
});

await app.UseOcelot();

await app.RunAsync();