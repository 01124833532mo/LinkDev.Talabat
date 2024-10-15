using Azure.Core;
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


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // logic excuted with the request
                await _next(httpContext);
                 //logic excuted with the response
                //if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                //{
                //    var respnse = new ApiResponse((int)HttpStatusCode.NotFound, $"the requested endpoint : {httpContext.Request.Path} is not found");
                //    await httpContext.Response.WriteAsync(respnse.ToString());
                //}
            }
            catch (Exception ex)
            {
                ApiResponse response;
                switch (ex)
                {
                    
                    case NotFoundExeption :

                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        httpContext.Response.ContentType = "application/json";
                        response = new ApiResponse(404, ex.Message);
                        await httpContext.Response.WriteAsync(response.ToString());

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

                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        httpContext.Response.ContentType = "application/json";

                        await httpContext.Response.WriteAsync(response.ToString());
                        break;
                }



            }
        }

    }
}
