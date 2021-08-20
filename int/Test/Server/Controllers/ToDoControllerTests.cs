using FluentAssertions;
using Integration.Sdk.Models;
using Integration.Utility;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Server.Controllers
{
    public class ToDoControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;

        public ToDoControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            this.webApplicationFactory = webApplicationFactory;
        }

        [Trait(nameof(Category), nameof(Category.Integration))]
        [Fact(DisplayName = "Should Get ToDo Items")]
        public async Task ShouldGetToDoItems()
        {
            HttpClient client = webApplicationFactory.CreateClient();
            string url = "/api/todo";

            HttpResponseMessage response = await client.GetAsync(url);

            response.Should().NotBeNull()
                .And.Match<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.OK);

            var items = await GetObjectFromResponse<IEnumerable<ToDoItem>>(response);

            items.Should().HaveCount(ToDoController.INITIAL_TODO_COUNT);
        }

        private async Task<T> GetObjectFromResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}