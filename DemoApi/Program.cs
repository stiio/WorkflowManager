using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.Core;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Data.Interceptors;
using Stio.WorkflowManager.DemoApi.Services;
using Stio.WorkflowManager.DemoApi.Services.FlowServices;

var builder = WebApplication.CreateBuilder(args);

ConfigureConfiguration(builder.Configuration, builder.Environment);

ConfigureServices(builder.Services, builder.Configuration);

var webApplication = builder.Build();

ConfigureMiddleware(webApplication, webApplication.Environment);

ConfigureEndpoints(webApplication);

webApplication.Run();

void ConfigureConfiguration(ConfigurationManager configuration, IWebHostEnvironment env)
{
    configuration.AddJsonFile("appsettings.json", false, true) // load base settings
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true) // load environment settings
        .AddJsonFile("appsettings.local.json", true, true) // load local settings
        .AddEnvironmentVariables();
}

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddScoped<TimeStampSaveChangesInterceptor>();
    services.AddDbContext<ApplicationDbContext>(opts =>
    {
        opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    });

    services.AddWorkflowManager<Workflow, WorkflowStep>(typeof(Program))
        .AddWorkflowStore<WorkflowStore>()
        .AddWorkflowStepStore<WorkflowStepStore>();

    services.AddScoped<UserService>();
    services.AddScoped<RelatedObjectFlowService>();
    services.AddScoped<SecondBlockFlowService>();
    services.AddScoped<ThirdBlockFlowService>();
    services.AddScoped<ReviewBlockService>();

    services.AddControllers()
        .AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddHttpContextAccessor();
}

void ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.EnsureDbExists();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
}

void ConfigureEndpoints(IEndpointRouteBuilder app)
{
    app.MapControllers();
}

/// <summary>
/// Program
/// </summary>
public partial class Program
{
}