using Api.Dtos.Paycheck;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests
{
    [Collection("ApiTests")]
    public class PaycheckIntegrationTests : IntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public PaycheckIntegrationTests(TestWebApplicationFactory applicationFactory)
            : base(applicationFactory)
        {
        }

        [Fact]
        public async Task WhenAskedForAPaycheck_ShouldReturnCorrectPaycheck()
        {
            var httpClient = ApplicationFactory.CreateClient();

            var response = await httpClient.GetAsync("/api/v1/paychecks/1");
            var paycheck = new GetPaycheckDto
            {
                Id = 1,
                EmployeeId = 1,
                Year = 2022,
                Salary = 75420.99m,
                Deductions = 500
            };
            await response.ShouldReturn(HttpStatusCode.OK, paycheck);
        }

        [Fact]
        public async Task WhenAskedForANonexistentPaycheck_ShouldReturn404()
        {
            var httpClient = ApplicationFactory.CreateClient();

            var response = await httpClient.GetAsync($"/api/v1/paychecks/{int.MinValue}");
            await response.ShouldReturn(HttpStatusCode.NotFound);
        }
    }
}
