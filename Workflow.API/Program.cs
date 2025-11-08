using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Workflow.API.DI;
using Workflow.API.JWT;
using Workflow.Domain.Entities.Identity;
using Workflow.Persistence;
using Workflow.Service.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Workflow.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Bearer {Token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});

builder.Services.AddAutoMapper(typeof(EntityToViewModelMappingProfile).Assembly);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<WorkflowDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var corsBuilder = new CorsPolicyBuilder();
corsBuilder.AllowAnyHeader();
corsBuilder.AllowAnyMethod();
corsBuilder.AllowAnyOrigin();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder.Build());
});

builder.Services.AddDIs();

builder.Services.AddIdentity<User, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan =
        TimeSpan.FromMinutes(Convert.ToInt32(configuration["Tokens:accessFailedWaitingMin"]));
    options.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(configuration["Tokens:accessFailedCount"]);
}
).AddDefaultTokenProviders().AddEntityFrameworkStores<WorkflowDbContext>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenHandlers.Clear();
    jwt.EventsType = typeof(JwtHandlerEvents);
});

builder.Services.AddAuthorization();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        if (feature != null)
        {
            var ex = feature.Error;
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

            logger.LogError(ex, "Unhandled exception occurred");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                Success = false,
                Message = "Internal server error",
                Detail = ex.Message
            });
        }
    });
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();