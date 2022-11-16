using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Data.Interceptors;

namespace Stio.WorkflowManager.DemoApi.Data;

public class ApplicationDbContext : DbContext
{
    private readonly TimeStampSaveChangesInterceptor? timeStampSaveChangesInterceptor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    [ActivatorUtilitiesConstructor]
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        TimeStampSaveChangesInterceptor timeStampSaveChangesInterceptor)
        : base(options)
    {
        this.timeStampSaveChangesInterceptor = timeStampSaveChangesInterceptor;
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Workflow> Workflows { get; set; } = null!;

    public DbSet<WorkflowStep> WorkflowSteps { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(new User[]
        {
            new User()
            {
                Id = new("AA9AFDAF-2C5D-4CA6-81A1-64D98CC56878"),
            },
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (this.timeStampSaveChangesInterceptor is not null)
        {
            optionsBuilder.AddInterceptors(this.timeStampSaveChangesInterceptor);
        }
    }
}