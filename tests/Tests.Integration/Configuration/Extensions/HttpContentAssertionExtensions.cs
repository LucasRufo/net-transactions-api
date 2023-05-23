using System.Net.Http.Json;

namespace Tests.Integration.Configuration.Extensions;

public static class HttpContentAssertionExtensions
{
    public static async void ShouldBeEquivalentTo<TContent, TExpectation>(this HttpContent content, TExpectation expectation) where TContent : class where TExpectation : class
    {
        var contentResponse = await content.ReadFromJsonAsync<TContent>();

        contentResponse.Should().BeEquivalentTo(expectation);
    }
}
