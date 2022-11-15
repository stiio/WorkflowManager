using Stio.WorkflowManager.DemoApi.Enums;
using Xunit.Abstractions;

namespace Stio.WorkflowManager.Test
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper testOutputHelper;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1()
        {
            this.testOutputHelper.WriteLine(nameof(Step.FirstBlockQuestion1));
        }
    }
}