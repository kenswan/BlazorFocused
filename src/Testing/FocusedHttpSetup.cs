using System;
using System.Net;

namespace BlazorFocused.Testing
{
    public class FocusedHttpSetup
    {
        private readonly FocusedHttpRequest request;
        private readonly Action<FocusedHttpRequest, HttpStatusCode, object> responseAction;

        public FocusedHttpSetup(
            FocusedHttpRequest request,
            Action<FocusedHttpRequest, HttpStatusCode, object> responseAction)
        {
            this.request = request;
            this.responseAction = responseAction;
        }

        public void ReturnsAsync(HttpStatusCode statusCode, object responseObject)
        {
            responseAction(request, statusCode, responseObject);
        }
    }
}
