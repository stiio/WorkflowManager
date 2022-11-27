using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.Test.Integration.TestCaseData;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Stio.WorkflowManager.Test.Integration.TestCases;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
[Collection("DemoApp")]
public class TestCase001 : IClassFixture<TestCase001Data>
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

    [Fact]
    [Priority(1)]
    public async Task Test_001_StartWorkflow()
    {
        var requestJson = this.testCase001Data.FirstBlockQuestion1Data.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/workflows/first_block/first_question")
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        var nextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);

        // Set workflowId
        this.testCase001Data.WorkflowId = nextStepResponse!.WorkflowId;

        // Set next step
        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Priority(2)]
    public async Task Test_002_GetFirstBlockSecondQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Priority(3)]
    public async Task Test_003_GoToPreviousStep()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.PreviousStepUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(4)]
    public async Task Test_004_GetFirstBlockFirstQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var firstBlockQuestion1Response = responseJson.FromJson<FirstBlockQuestion1Response>(this.jsonOptions);

        Assert.Equal(this.testCase001Data.FirstBlockQuestion1Data.Agree, firstBlockQuestion1Response!.Agree);
        Assert.Equal(this.testCase001Data.FirstBlockQuestion1Data.FirstName, firstBlockQuestion1Response.FirstName);
        Assert.Equal(this.testCase001Data.FirstBlockQuestion1Data.LastName, firstBlockQuestion1Response.LastName);
    }

    [Fact]
    [Priority(5)]
    public async Task Test_005_EditFirstBlockFirstQuestion()
    {
        var requestJson = this.testCase001Data.FirstBlockQuestion1Data.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/workflows/first_block/first_question?workflowId={this.testCase001Data.WorkflowId}")
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(6)]
    public async Task Test_006_GetFirstBlockSecondQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Priority(7)]
    public async Task Test_007_EditFirstBlockSecondQuestion()
    {
        var requestJson = this.testCase001Data.FirstBlockQuestion2Data.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(8)]
    public async Task Test_008_GetSecondBlockFirstQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var secondBlockFirstQuestionResponse = responseJson.FromJson<SecondBlockQuestion1Response>(this.jsonOptions);

        Assert.Equal($"{this.testCase001Data.FirstBlockQuestion1Data.FirstName} {this.testCase001Data.FirstBlockQuestion1Data.LastName}", secondBlockFirstQuestionResponse!.FullName);
        Assert.Empty(secondBlockFirstQuestionResponse.RelatedObjects);
    }

    [Fact]
    [Priority(9)]
    public async Task Test_009_AddFirstRelatedObject()
    {
        var requestJson = new RelatedObjectCreateRequest()
        {
            WorkflowId = this.testCase001Data.WorkflowId,
            Name = this.testCase001Data.RelatedObjects[0].Name!,
        }.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/related_objects")
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var relatedObject = responseJson.FromJson<RelatedObjectDto>(this.jsonOptions);

        this.testCase001Data.RelatedObjects[0].Id = relatedObject!.Id;
    }

    [Fact]
    [Priority(10)]
    public async Task Test_010_GetSecondBlockFirstQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var secondBlockFirstQuestionResponse = responseJson.FromJson<SecondBlockQuestion1Response>(this.jsonOptions);

        Assert.Equal($"{this.testCase001Data.FirstBlockQuestion1Data.FirstName} {this.testCase001Data.FirstBlockQuestion1Data.LastName}", secondBlockFirstQuestionResponse!.FullName);
        Assert.Single(secondBlockFirstQuestionResponse.RelatedObjects);
    }

    [Fact]
    [Priority(11)]
    public async Task Test_011_AddSecondRelatedObject()
    {
        var requestJson = new RelatedObjectCreateRequest()
        {
            WorkflowId = this.testCase001Data.WorkflowId,
            Name = this.testCase001Data.RelatedObjects[1].Name!,
        }.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/related_objects")
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var relatedObject = responseJson.FromJson<RelatedObjectDto>(this.jsonOptions);

        this.testCase001Data.RelatedObjects[1].Id = relatedObject!.Id;
    }

    [Fact]
    [Priority(12)]
    public async Task Test_012_GetSecondBlockFirstQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var secondBlockFirstQuestionResponse = responseJson.FromJson<SecondBlockQuestion1Response>(this.jsonOptions);

        Assert.Equal($"{this.testCase001Data.FirstBlockQuestion1Data.FirstName} {this.testCase001Data.FirstBlockQuestion1Data.LastName}", secondBlockFirstQuestionResponse!.FullName);
        Assert.Equal(2, secondBlockFirstQuestionResponse.RelatedObjects.Length);
    }

    [Fact]
    [Priority(13)]
    public async Task Test_013_AddThirdRelatedObject()
    {
        var requestJson = new RelatedObjectCreateRequest()
        {
            WorkflowId = this.testCase001Data.WorkflowId,
            Name = this.testCase001Data.RelatedObjects[2].Name!,
        }.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/related_objects")
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var relatedObject = responseJson.FromJson<RelatedObjectDto>(this.jsonOptions);

        this.testCase001Data.RelatedObjects[2].Id = relatedObject!.Id;
    }

    [Fact]
    [Priority(14)]
    public async Task Test_014_GetSecondBlockFirstQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var secondBlockFirstQuestionResponse = responseJson.FromJson<SecondBlockQuestion1Response>(this.jsonOptions);

        Assert.Equal($"{this.testCase001Data.FirstBlockQuestion1Data.FirstName} {this.testCase001Data.FirstBlockQuestion1Data.LastName}", secondBlockFirstQuestionResponse!.FullName);
        Assert.Equal(3, secondBlockFirstQuestionResponse.RelatedObjects.Length);
    }

    [Fact]
    [Priority(15)]
    public async Task Test_015_EditSecondBlockFirstQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(16)]
    public async Task Test_016_GetSecondBlockSecondQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Priority(17)]
    public async Task Test_017_EditSecondBlockSecondQuestion()
    {
        var requestJson = this.testCase001Data.SecondBlockQuestion2Data.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(18)]
    public async Task Test_018_GetSecondBlockThirdQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Priority(19)]
    public async Task Test_019_EditSecondBlockThirdQuestion()
    {
        var requestJson = this.testCase001Data.SecondBlockQuestion3Data.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(20)]
    public async Task Test_020_GetSecondBlockFourthQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var secondBlockQuestion4Response = responseJson.FromJson<SecondBlockQuestion4Response>(this.jsonOptions);

        Assert.Equal(this.testCase001Data.RelatedObjects[0].Name, secondBlockQuestion4Response!.Name);
    }

    [Fact]
    [Priority(21)]
    public async Task Test_021_EditSecondBlockFourthQuestion()
    {
        var requestJson = this.testCase001Data.SecondBlockQuestion4Data[0].ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(22)]
    public async Task Test_022_GetSecondBlockFourthQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var secondBlockQuestion4Response = responseJson.FromJson<SecondBlockQuestion4Response>(this.jsonOptions);

        Assert.Equal(this.testCase001Data.RelatedObjects[1].Name, secondBlockQuestion4Response!.Name);
    }

    [Fact]
    [Priority(23)]
    public async Task Test_023_EditSecondBlockFourthQuestion()
    {
        var requestJson = this.testCase001Data.SecondBlockQuestion4Data[1].ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(24)]
    public async Task Test_024_GetSecondBlockFourthQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var secondBlockQuestion4Response = responseJson.FromJson<SecondBlockQuestion4Response>(this.jsonOptions);

        Assert.Equal(this.testCase001Data.RelatedObjects[2].Name, secondBlockQuestion4Response!.Name);
    }

    [Fact]
    [Priority(25)]
    public async Task Test_025_EditSecondBlockFourthQuestion()
    {
        var requestJson = this.testCase001Data.SecondBlockQuestion4Data[2].ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(26)]
    public async Task Test_026_GetThirdBlockFirstQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var thirdBlockQuestion1Response = responseJson.FromJson<ThirdBlockQuestion1Response>(this.jsonOptions);

        Assert.Equal(3, thirdBlockQuestion1Response!.RelatedObjects.Length);
    }

    [Fact]
    [Priority(27)]
    public async Task Test_027_EditThirdBlockFirstQuestion()
    {
        var requestJson = this.testCase001Data.ThirdBlockQuestion1Data.ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(28)]
    public async Task Test_028_GetThirdBlockSecondQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var thirdBlockQuestion2Response = responseJson.FromJson<ThirdBlockQuestion2Response>(this.jsonOptions);

        Assert.Equal(this.testCase001Data.RelatedObjects[0].Name, thirdBlockQuestion2Response!.Name);
    }

    [Fact]
    [Priority(29)]
    public async Task Test_029_EditThirdBlockFirstQuestion()
    {
        var requestJson = this.testCase001Data.ThirdBlockQuestion2Data[0].ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(30)]
    public async Task Test_030_GetThirdBlockSecondQuestion()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var thirdBlockQuestion2Response = responseJson.FromJson<ThirdBlockQuestion2Response>(this.jsonOptions);

        Assert.Equal(this.testCase001Data.RelatedObjects[2].Name, thirdBlockQuestion2Response!.Name);
    }

    [Fact]
    [Priority(31)]
    public async Task Test_031_EditThirdBlockFirstQuestion()
    {
        var requestJson = this.testCase001Data.ThirdBlockQuestion2Data[1].ToJson(this.jsonOptions);

        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        };

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nRequest Content: {requestJson}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(32)]
    public async Task Test_032_GetReviewBlockReview()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Priority(33)]
    public async Task Test_033_EditReviewBlockReview()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        this.testCase001Data.NextStepResponse = responseJson.FromJson<NextStepResponse>(this.jsonOptions);
    }

    [Fact]
    [Priority(34)]
    public async Task Test_034_GetReviewBlockCompleted()
    {
        using var client = this.demoApp.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, this.testCase001Data.RequestUri);

        using var response = await client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        this.testOutputHelper.WriteLine($"Request: {request}\n\nResponse: {response}\n\nResponseContent: {responseJson}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}