// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Net;

namespace BlazorFocused.Tools;

/// <summary>
/// Handles configuration/simulated response returned for a request from <see cref="ISimulatedHttp"/>
/// </summary>
public interface ISimulatedHttpSetup
{
    /// <summary>
    /// Configures the expected results of "Setup" request/>
    /// </summary>
    /// <param name="statusCode">Simulated http response status</param>
    /// <param name="responseObject">Simulated deserialized object in http response body</param>
    void ReturnsAsync(HttpStatusCode statusCode, object responseObject);
}
