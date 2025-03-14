using Asp.Versioning;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Serilog Configurations
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddApiVersioning(cfg =>
{
    cfg.DefaultApiVersion = new ApiVersion(1, 0);
    cfg.AssumeDefaultVersionWhenUnspecified = true;
    cfg.ReportApiVersions = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Catalog.API", Version = "v1" });
});

//Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Register Mediatr
var assemblies = new[] {
    Assembly.GetExecutingAssembly(),
    typeof(GetAllBrandsHandler).Assembly
};

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

//Register Application Services
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, ProductRepository>();
builder.Services.AddScoped<ITypesRepository, ProductRepository>();

//Identity Server

//var userPolicy = new AuthorizationPolicyBuilder()
//    .RequireAuthenticatedUser()
//    .Build();

//builder.Services.AddControllers(config =>
//{
//    config.Filters.Add(new AuthorizeFilter(userPolicy));
//});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>{
//        options.Authority = "https://localhost:9009";
//        options.Audience = "Catalog";
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanRead", policy => policy.RequireClaim("scope", "catalogapi.read"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
