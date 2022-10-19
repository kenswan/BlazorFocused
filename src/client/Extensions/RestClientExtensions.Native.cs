using BlazorFocused.Client;

namespace BlazorFocused.Extensions;
public static partial class RestClientExtensions
{
    public static async Task<RestClientResponse<T>> SendAsync<T>(this IRestClient restClient, HttpRequestMessage httpRequestMessage)
    {
        RestClientHttpResponse<T> restClientHttpResponse = new();
        Exception exception = default;

        try
        {
            if (restClient is BaseRestClient baseRestClient)
                restClientHttpResponse =
                    await baseRestClient.SendAndDeserializeAsync<T>(httpRequestMessage);
            else
                ThrowUnqualifiedRestClient(restClient);
        }
        catch (Exception ex)
        {
            exception = ex;

            if (ex is RestClientHttpException restClientHttpException)
                restClientHttpResponse.StatusCode = restClientHttpException.StatusCode;
        }

        return new RestClientResponse<T>
        {
            Exception = exception,
            Headers = restClientHttpResponse.Headers,
            StatusCode = restClientHttpResponse.StatusCode.GetValueOrDefault(),
            Value = restClientHttpResponse.Content
        };
    }

    public static async Task<RestClientTask> SendAsync(this IRestClient restClient, HttpRequestMessage httpRequestMessage)
    {
        RestClientHttpResponse restClientHttpResponse = new();
        Exception exception = default;

        try
        {
            if (restClient is BaseRestClient baseRestClient)
                restClientHttpResponse = await baseRestClient.SendAndTaskAsync(httpRequestMessage);
            else
                ThrowUnqualifiedRestClient(restClient);
        }
        catch (Exception ex)
        {
            exception = ex;

            if (ex is RestClientHttpException restClientHttpException)
                restClientHttpResponse.StatusCode = restClientHttpException.StatusCode;
        }

        return new RestClientTask
        {
            Exception = exception,
            Headers = restClientHttpResponse.Headers,
            StatusCode = restClientHttpResponse.StatusCode.GetValueOrDefault(),
        };
    }

    public static async Task<HttpResponseMessage> BaseSendAsync(this IRestClient restClient, HttpRequestMessage httpRequestMessage)
    {
        HttpResponseMessage responseMessage = null;

        if (restClient is BaseRestClient baseRestClient)
            responseMessage = await baseRestClient.StandardSendAsync(httpRequestMessage);
        else
            ThrowUnqualifiedRestClient(restClient);

        return responseMessage;
    }
}
