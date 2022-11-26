using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.Test.TestCaseData;

public class TestCase001Data
{
    public Guid WorkflowId { get; set; }

    public NextStepResponse? NextStepResponse { get; set; }

    public string RequestUri => this.NextStepResponse!.ToRequestUri();

    public string PreviousStepUri => $"api/workflows/{this.WorkflowId}/previous_step";

    public FirstBlockQuestion1Data FirstBlockQuestion1Data => new FirstBlockQuestion1Data()
    {
        FirstName = "FN1",
        LastName = "LN1",
        Agree = true,
    };

    public FirstBlockQuestion2Data FirstBlockQuestion2Data => new FirstBlockQuestion2Data()
    {
        Amount = 501,
    };

    public RelatedObjectDto[] RelatedObjects = new RelatedObjectDto[]
    {
        new RelatedObjectDto()
        {
            Id = Guid.Empty,
            Name = "RO_0",
        },
        new RelatedObjectDto()
        {
            Id = Guid.Empty,
            Name = "RO_1",
        },
        new RelatedObjectDto()
        {
            Id = Guid.Empty,
            Name = "RO_2",
        },
    };

    public SecondBlockQuestion2Data SecondBlockQuestion2Data => new SecondBlockQuestion2Data()
    {
        SomeAnswer = true,
        SomeInput = "some text",
    };

    public SecondBlockQuestion3Data SecondBlockQuestion3Data => new SecondBlockQuestion3Data()
    {
        Checked = new string[] { "1", "2" },
    };

    public SecondBlockQuestion4Data[] SecondBlockQuestion4Data => new SecondBlockQuestion4Data[]
    {
        new SecondBlockQuestion4Data()
        {
            Answer = false,
        },
        new SecondBlockQuestion4Data()
        {
            Amount = 10,
            Answer = true,
        },
        new SecondBlockQuestion4Data()
        {
            Amount = 20,
            Answer = true,
        },
    };

    public ThirdBlockQuestion1Data ThirdBlockQuestion1Data => new ThirdBlockQuestion1Data()
    {
        CheckedIds = new Guid[] { this.RelatedObjects[0].Id, this.RelatedObjects[2].Id }
    };

    public ThirdBlockQuestion2Data[] ThirdBlockQuestion2Data => new ThirdBlockQuestion2Data[]
    {
        new ThirdBlockQuestion2Data()
        {
            Agree = true,
            Count = 3,
        },
        new ThirdBlockQuestion2Data()
        {
            Agree = false,
            Count = 0,
        },
    };
}