using LinkDev.Talabat.Apis.Controllers.Errors;
using LinkDev.Talabat.Apis.Controllers.Exeptions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace LinkDev.Talabat.Apis.Middlewares
{
    public class CustomExeptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExeptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public CustomExeptionHandlerMiddleware(RequestDelegate next,ILogger<CustomExeptionHandlerMiddleware> logger,IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }


        public async Task InvokeAsync(HttpContext httpContent)
        {
            try
            {
                // logic excuted with the request
                await _next(httpContent);
                // logic excuted with the response
            }
            catch (Exception ex)
            {
                ApiResponse response;
                switch (ex)
                {
                    
                    case NotFoundExeption :

                        httpContent.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        httpContent.Response.ContentType = "application/json";
                        response = new ApiResponse(404, ex.Message);
                        await httpContent.Response.WriteAsync(response.ToString());

                        break;

                    default:


                        if (_env.IsDevelopment())
                        {

                            //development mode

                            _logger.LogError(ex, ex.Message);

                            response = new ApiExeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString());
                        }

                        else
                        {
                            // production mode
                            // log exeption details t (file | text)

                            response = new ApiExeptionResponse((int)HttpStatusCode.InternalServerError);
                        }

                        httpContent.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        httpContent.Response.ContentType = "application/json";

                        await httpContent.Response.WriteAsync(response.ToString());
                        break;
                }



            }
        }

    }
}
