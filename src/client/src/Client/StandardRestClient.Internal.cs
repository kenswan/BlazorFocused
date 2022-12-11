// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Client;

internal partial class StandardRestClient
{
    private async Task<T> ExtractOrThrowAsync<T>(HttpMethod httpMethod, string url, object data = null)
    {
        RestClientResponse<T> restClientResponse = await SendAsync<T>(httpMethod, url, data);

        return restClientResponse.IsSuccess ? restClientResponse.Value : throw restClientResponse.Exception;
    }

    private async Task ExtractOrThrowAsync(HttpMethod httpMethod, string url, object data = null)
    {
        RestClientTask restClientTask = await SendAsync(httpMethod, url, data);

        if (!restClientTask.IsSuccess)
        {
            throw restClientTask.Exception;
        }
    }
}
