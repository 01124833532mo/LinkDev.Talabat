using LinkDev.Talabat.Core.Application.Abstraction.Common.Contract.Infrastructure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Cache_Sevice
{
	public class ResponseCacheService(IConnectionMultiplexer redis) : IResponseCacheService
	{
		private readonly IDatabase _database = redis.GetDatabase();

		public async Task CacheResponseAsynce(string key, object respons, TimeSpan TimeToLive)
		{
			if (respons is null) return ;

			var serelizedoption= new JsonSerializerOptions() { PropertyNamingPolicy= JsonNamingPolicy.CamelCase };
			var serelizerrespons = JsonSerializer.Serialize(respons, serelizedoption);

			await _database.StringSetAsync(key, serelizerrespons, TimeToLive);
		}

		public async Task<string?> GetCachedResponsAsync(string key)
		{
			var response=await _database.StringGetAsync( key);

			if (response.IsNull) return null;

			return response;

		}
	}
}
