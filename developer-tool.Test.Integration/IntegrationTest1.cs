using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Models;
using Newtonsoft.Json;
using Xunit;
using WebApi.Interfaces;

namespace TestIntegration
{
    public class IntegrationTest1 : IntegrationTestBase
    {
        private readonly HttpClient _client;
        // private readonly ITestService _testService;

        public IntegrationTest1() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("Steve Smith")]
        [InlineData("Neil Gaiman")]
        public async Task Get_ShouldReturnAllItems(string firstName)
        {
            var response = await _client.GetAsync($"/api/test");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Test>>(stringResponse).ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.Count(a => a.FirstName == firstName));
        }

        [Fact]
        public async Task Get_ShouldReturnCountEquals2()
        {
            var response = await _client.GetAsync($"/api/test");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Test>>(stringResponse).ToList();

            Assert.Equal(2, result.Count());
        }
    }
}
