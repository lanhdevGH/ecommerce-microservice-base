using Microsoft.AspNetCore.Http;

namespace Common.Logging
{
    public class HttpContextProvider
    {
        public static IHttpContextAccessor? Accessor { get; set; }
    }
}
