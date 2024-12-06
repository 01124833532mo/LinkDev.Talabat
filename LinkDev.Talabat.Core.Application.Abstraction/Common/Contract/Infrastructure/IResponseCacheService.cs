using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Common.Contract.Infrastructure
{
	public interface IResponseCacheService
	{
		Task CacheResponseAsynce(string key, object respons,TimeSpan TimeToLive );

		Task<string?> GetCachedResponsAsync(string key);

	}
}
