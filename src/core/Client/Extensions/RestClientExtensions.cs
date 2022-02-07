using System.Net;

namespace BlazorFocused.Client.Extensions
{
    public static partial class RestClientExtensions
    {
        private static async Task<RestClientResponse<T>> GetRestClientResponse<T>(
            IRestClient restClient, HttpMethod method, string url, object data = null)
        {
            HttpStatusCode? httpStatusCode = null;
            T value = default;
            Exception exception = default;

            try
            {
                if(restClient is BaseRestClient baseRestClient)
                    (httpStatusCode, value) =
                        await baseRestClient.SendAndDeserializeAsync<T>(method, url, data);
                else
                    ThrowUnqualifiedRestClient(restClient);
            }
            catch (Exception ex)
            {
                exception = ex;
                
                if (ex is RestClientHttpException restClientHttpException)
                    httpStatusCode = restClientHttpException.StatusCode;
            }

            return new RestClientResponse<T>
            {
                Exception = exception,
                StatusCode = httpStatusCode.GetValueOrDefault(),
                Value = value
            };
        }

        private static async Task<RestClientTask> GetRestClientTask(
            IRestClient restClient, HttpMethod method, string url, object data = null)
        {
            HttpStatusCode? httpStatusCode = null;
            Exception exception = default;

            try
            {
                if (restClient is BaseRestClient baseRestClient)
                    httpStatusCode = await baseRestClient.SendAndTaskAsync(method, url, data);
                else
                    ThrowUnqualifiedRestClient(restClient);
            }
            catch (Exception ex)
            {
                exception = ex;

                if (ex is RestClientHttpException restClientHttpException)
                    httpStatusCode = restClientHttpException.StatusCode;
            }

            return new RestClientTask
            {
                Exception = exception,
                StatusCode = httpStatusCode.GetValueOrDefault(),
            };
        }

        private static void ThrowUnqualifiedRestClient(IRestClient restClient)
        {
            throw new RestClientException(
                        $"Operation not allowed by IRestClient of type {restClient.GetType().FullName}");
        }
    }
}
