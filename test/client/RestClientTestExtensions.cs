// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools;

namespace BlazorFocused;

internal static partial class RestClientTestExtensions
{
    public static ISimulatedHttpSetup GetHttpSetup(this ISimulatedHttp simulatedHttp,
            HttpMethod httpMethod, string url, object request)
    {
        return httpMethod switch
        {
            { } when httpMethod == HttpMethod.Delete => simulatedHttp.SetupDELETE(url),
            { } when httpMethod == HttpMethod.Get => simulatedHttp.SetupGET(url),
            { } when httpMethod == HttpMethod.Patch => simulatedHttp.SetupPATCH(url, request),
            { } when httpMethod == HttpMethod.Post => simulatedHttp.SetupPOST(url, request),
            { } when httpMethod == HttpMethod.Put => simulatedHttp.SetupPUT(url, request),
            _ => throw new NotImplementedException($"HttpMethod {httpMethod} not implemented")
        };
    }

    public static void VerifyWasCalled(this ISimulatedHttp simulatedHttp,
            HttpMethod httpMethod, string url = default, object content = default)
    {
        Action action = httpMethod switch
        {
            { } when httpMethod == HttpMethod.Delete =>
                () => simulatedHttp.VerifyDELETEWasCalled(url),
            { } when httpMethod == HttpMethod.Get =>
                () => simulatedHttp.VerifyGETWasCalled(url),
            { } when httpMethod == HttpMethod.Patch =>
                () => simulatedHttp.VerifyPATCHWasCalled(url, content),
            { } when httpMethod == HttpMethod.Post =>
                () => simulatedHttp.VerifyPOSTWasCalled(url, content),
            { } when httpMethod == HttpMethod.Put =>
                () => simulatedHttp.VerifyPUTWasCalled(url, content),
            _ => throw new NotImplementedException($"HttpMethod {httpMethod} not implemented")
        };

        action.Invoke();
    }
}
