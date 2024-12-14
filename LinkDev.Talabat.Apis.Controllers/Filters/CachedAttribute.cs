using LinkDev.Talabat.Core.Application.Abstraction.Common.Contract.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Apis.Controllers.Filters
{
	internal class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timetoliveinseconde;

		public CachedAttribute(int timetoliveinseconde)
        {
			_timetoliveinseconde = timetoliveinseconde;
		}
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var responseCachedServices = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

			var cachkey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

			var response = await responseCachedServices.GetCachedResponsAsync(cachkey);

			if (!string.IsNullOrEmpty(response)) {

				var resulte = new ContentResult()
				{
					Content = response,
					ContentType = "application/json",
					StatusCode = 200
				};
				
				context.Result= resulte;

				return; 
			}

			var excutedActionContext=	await next.Invoke();


			if(excutedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
			{
				await responseCachedServices.CacheResponseAsynce(cachkey,okObjectResult.Value,TimeSpan.FromSeconds(_timetoliveinseconde));
			}
		}

		private string GenerateCacheKeyFromRequest(HttpRequest request)
		{
			var keyBuldier = new StringBuilder();

			keyBuldier.Append(request.Path);

			foreach (var (key, value) in request.Query.OrderBy(x=>x.Key)) {

				keyBuldier.Append($"|{key}-{value}");
			
			}

			return keyBuldier.ToString();
		}
	}
}
