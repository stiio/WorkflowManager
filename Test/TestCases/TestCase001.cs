using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.Test.TestCaseData;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Stio.WorkflowManager.Test.TestCases;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class TestCase001 : IClassFixture<DemoApp>, IClassFixture<TestCase001Data>
{
    private readonly DemoApp demoApp;
    private readonly JsonSerializerOptions jsonOptions;
    private readonly TestCase001Data testCase001Data;
    private readonly ITestOutputHelper testOutputHelper;

    public TestCase001(DemoApp demoApp, TestCase001Data testCase001Data, ITestOutputHelper testOutputHelper)
    {
        this.demoApp = demoApp;
        this.testOutputHelper = testOutputHelper;
        this.testCase001Data = testCase001Data;
        this.jsonOptions = demoApp.Services.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;
    }

    [Fact, Priority(0)]
    public async Task Test_1_StartWorkflow()
    {
        var requestJson = new FirstBlockQuestion1Data()
        {
            FirstName = "Test",
            LastName = "Test",
            Agree = true,
        }.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/workflows/first_block/first_question")
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        var nextStep = responseJson.FromJson<NextStepResponse>(this.jsonOptions);

        // Set workflowId
        this.testCase001Data.WorkflowId = nextStep!.WorkflowId;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact, Priority(1)]
    public async Task Test_2_GetFirstBlockSecondQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/workflows/{this.testCase001Data.WorkflowId}/first_block/second_question");

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}