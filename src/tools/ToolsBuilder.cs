using BlazorFocused.Tools.Http;
using BlazorFocused.Tools.Logger;
using Microsoft.Extensions.Logging;

namespace BlazorFocused.Tools
{
    public static class ToolsBuilder
    {
        public static ITestLogger<T> CreateTestLogger<T>(
            Action<LogLevel, string, Exception> logAction = null) =>
                new TestLogger<T>(logAction);

        public static ISimulatedHttp CreateSimulatedHttp(string baseAddress = null) =>
            baseAddress is not null ?
                new SimulatedHttp(baseAddress) : new SimulatedHttp();
    }
}
