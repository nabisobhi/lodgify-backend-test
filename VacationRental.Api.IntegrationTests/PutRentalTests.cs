using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.IntegrationTests
{
    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAGetReturnsTheUpdatedRental()
        {
            var postRrequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1,
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRrequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var putRequest = new RentalUpdateModel
            {
                Id = postResult.Id,
                Units = 5,
                PreparationTimeInDays = 4,
            };

            ResultViewModel putResult;
            using (var postResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                putResult = await postResponse.Content.ReadAsAsync<ResultViewModel>();
                Assert.True(putResult.IsSuccessful);
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(putRequest.Units, getResult.Units);
                Assert.Equal(putRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }
    }
}
