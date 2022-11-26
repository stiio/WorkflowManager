using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stio.WorkflowManager.DemoApi.Data;

namespace Stio.WorkflowManager.Test;

public class DemoApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlTestcontainer postgreSqlTestcontainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration()
        {
            Database = "sampleWorkflowManager",
            Username = "postgres",
            Password = "postgres",
            Port = 5555,
        })
        .WithAutoRemove(true)
        .WithImage("postgres:13.2")
        .Build();

    protected override void ConfigureClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseNpgsql(this.postgreSqlTestcontainer.ConnectionString);
            });
        });
    }

    public async Task InitializeAsync()
    {
        await this.postgreSqlTestcontainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await this.postgreSqlTestcontainer.DisposeAsync();
        await base.DisposeAsync();
    }
}