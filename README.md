[![https://www.nuget.org/packages/Stio.WorkflowManager](https://img.shields.io/nuget/v/Stio.WorkflowManager)](https://www.nuget.org/packages/Stio.WorkflowManager/)

# WorkflowManager

# Start

<details>
<summary>1. Implement IWorkflow and IWorkflowStep interfaces:</summary>

```csharp
public class Workflow : IWorkflow, ITimeStamp
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public string? Name { get; set; }

    public ICollection<WorkflowStep>? WorkflowSteps { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
```

```csharp
public class WorkflowStep : IWorkflowStep, ITimeStamp
{
    public Guid Id { get; set; }

    public Workflow? Workflow { get; set; }

    public Guid WorkflowId { get; set; }

    public string? Data { get; set; }

    public string? Payload { get; set; }

    public string? StepKey { get; set; }

    public string? PreviousStepKey { get; set; }

    public bool IsSoftDelete { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
```

</details>

<details>
<summary>2. Implement IWorkflowStore and IWorkflowStepStore inrerfaces:</summary>

```csharp
public class WorkflowStore : IWorkflowStore<Workflow>
{
    private readonly ApplicationDbContext context;

    public WorkflowStore(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Workflow?> FindById(Guid workflowId)
    {
        return await this.context.Workflows.FirstOrDefaultAsync(workflow => workflow.Id == workflowId);
    }
}
```

```csharp
public class WorkflowStepStore : IWorkflowStepStore<WorkflowStep>
{
    private readonly ApplicationDbContext context;

    public WorkflowStepStore(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<WorkflowStep> Create(WorkflowStep workflowStep)
    {
        await this.context.WorkflowSteps.AddAsync(workflowStep);
        await this.context.SaveChangesAsync();

        return workflowStep;
    }

    public async Task Update(WorkflowStep workflowStep)
    {
        this.context.WorkflowSteps.Update(workflowStep);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRange(WorkflowStep[] workflowSteps)
    {
        this.context.WorkflowSteps.UpdateRange(workflowSteps);
        await context.SaveChangesAsync();
    }

    public async Task<WorkflowStep[]> ListStepsByWorkflowId(Guid workflowId)
    {
        return await this.context.WorkflowSteps
            .Where(workflowStep => workflowStep.WorkflowId == workflowId)
            .AsNoTracking()
            .ToArrayAsync();
    }
}
```

</details>

<details>
<summary>3. Register WorkflowManagerFactory in DI:</summary>

```csharp
services.AddWorkflowManager<Workflow, WorkflowStep>(typeof(Program))
    .AddWorkflowStore<WorkflowStore>()
    .AddWorkflowStepStore<WorkflowStepStore>();
```

</details>

<details>
<summary>4. Create steps:</summary>

StepAttribute - the unique identifier of the step. Required attribute.  
The step must be inherited from:
- BaseStep<TWorkflow, TWorkflowStep>
- or BaseStep<TWorkflow, TWorkflowStep, TData>
- or BaseStep<TWorkflow, TWorkflowStep, TData, TPayload>

The step should implement the INextStep interface if it is not a dead-end step.  
You can use DI in your steps.

```csharp
[Step(nameof(Step.FirstBlockQuestion1))]
public class FirstBlockQuestion1Step : BaseStep<Workflow, WorkflowStep, FirstBlockQuestion1Data>,
    INextStep
{
    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new FirstBlockQuestion1Response()
        {
            FirstName = this.Data?.FirstName,
            LastName = this.Data?.LastName,
            Agree = this.Data?.Agree,
        });
    }

    public Task<NextStepResult> Next()
    {
        return Task.FromResult(NextStepResult.Create(Step.FirstBlockQuestion2.ToString()));
    }
}
```

```csharp
[Step(nameof(Step.FirstBlockQuestion2))]
public class FirstBlockQuestion2Step : BaseStep<Workflow, WorkflowStep, FirstBlockQuestion2Data>
{
    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new FirstBlockQuestion2Response()
        {
            Amount = this.Data?.Amount,
        });
    }
}
```

</details>

<details>
<summary>5. Usage</summary>

```csharp
[ApiController]
[Route("api/workflows/{workflowId}/first_block")]
public class FirstBlockController : ControllerBase
{
    private readonly IWorkflowManagerFactory<Workflow, WorkflowStep> workflowManagerFactory;
    private readonly ApplicationDbContext applicationDbContext;
    private readonly UserService userService;

    public FirstBlockController(
        ICustomWorkflowManagerFactory workflowManagerFactory,
        ApplicationDbContext applicationDbContext,
        UserService userService)
    {
        this.workflowManagerFactory = workflowManagerFactory;
        this.applicationDbContext = applicationDbContext;
        this.userService = userService;
    }

    [HttpGet("first_question")]
    public async Task<ActionResult<FirstBlockQuestion1Response>> GetFirstQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<FirstBlockQuestion1Response>();
    }

    // Start workflow or edit step if workflowId is provided
    [HttpPost("~/api/workflows/first_block/first_question")]
    public async Task<ActionResult<NextStepResponse>> EditFirstQuestion(Guid? workflowId, [Required] FirstBlockQuestion1Data request)
    {
        WorkflowManager<Workflow, WorkflowStep> workflowManager;
        if (workflowId is null)
        {
            var workflow = new Workflow()
            {
                UserId = this.userService.UserId,
                Name = "Some name",
            };

            await this.applicationDbContext.Workflows.AddAsync(workflow);
            await this.applicationDbContext.SaveChangesAsync();

            workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflow.Id);
        }
        else
        {
            workflowManager = await this.workflowManagerFactory.CreateWorkflowManager((Guid)workflowId);
        }

        StepKey nextStepKey;
        if (workflowManager.HasSteps())
        {
            if (!workflowManager.IsLastStep<FirstBlockQuestion1Step>())
            {
                return this.BadRequest(workflowManager.CreateWrongStepResponse());
            }

            nextStepKey = await workflowManager.Next(request);
        }
        else
        {
            nextStepKey = await workflowManager.Start<FirstBlockQuestion1Step, FirstBlockQuestion1Data>(request);
        }

        return this.Ok(nextStepKey.ToNextStepResponse(workflowManager.Workflow.Id));
    }

    [HttpGet("second_question")]
    public async Task<ActionResult<FirstBlockQuestion2Response>> GetSecondQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion2Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<FirstBlockQuestion2Response>();
    }

    [HttpPatch("second_question")]
    public async Task<ActionResult<NextStepResponse>> EditSecondQuestion(Guid workflowId, [Required] FirstBlockQuestion2Data request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion2Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }
        
        var nextStepKey = await workflowManager.Next(request);

        return nextStepKey.ToNextStepResponse(workflowId);
    }
}
```

</details>

See DemoApi project for a more complete example.
