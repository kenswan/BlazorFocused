using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Integration.Server;
using Integration.Test.Utility;
using Integration.Shared.Models;
using System.Collections.Generic;
using Integration.Server.Controllers;

namespace Integration.Test.Server.Controllers
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
            var content = await GetObjectFromResponse<IEnumerable<ToDoItem>>(response);

            response.Should().NotBeNull()
                .And.Match<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.OK);

            Assert.Equal(ToDoController.INITIAL_TODO_COUNT, content.Count());
        }

        private async Task<T> GetObjectFromResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}