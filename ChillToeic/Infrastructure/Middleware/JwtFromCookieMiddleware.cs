using System.Net;

namespace ChillToeic.Infrastructure.Middleware
{
    public class JwtFromCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtFromCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Lấy JWT từ cookie
            IRequestCookieCollection cookies = context.Request.Cookies;
            string jwt = null;
            foreach (var cookie in cookies)
            {   if (cookie.Key == "jwt")
                {
                    jwt = cookie.Value;
                    break;
                }
               
            }
            // Nếu JWT tồn tại, nhúng nó vào header Authorization
            if (!string.IsNullOrEmpty(jwt))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + jwt);
            }

            // Chuyển tiếp yêu cầu đến middleware tiếp theo trong pipeline
            await _next(context);
        }
    }
}
