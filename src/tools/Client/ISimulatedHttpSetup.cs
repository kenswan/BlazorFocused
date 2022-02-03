using System.Net;

namespace BlazorFocused.Tools.Client
{
    /// <summary>
    /// Handles configuration/simulated response returned for a request from <see cref="ISimulatedHttp"/>
    /// </summary>
    public interface ISimulatedHttpSetup
    {
        /// <summary>
        /// Configures the expected results of <see cref="ISimulatedHttp.Setup(System.Net.Http.HttpMethod, string)"/>
        /// </summary>
        /// <param name="statusCode">Simulated http response status</param>
        /// <param name="responseObject">Simulated deserialized object in http response body</param>
        void ReturnsAsync(HttpStatusCode statusCode, object responseObject);
    }
}
