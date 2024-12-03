using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors
{
    internal class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public AuditInterceptor(ILoggedInUserService loggedInUserService)
        {
            _loggedInUserService = loggedInUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntites(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			UpdateEntites(eventData.Context);

			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}





        private void UpdateEntites(DbContext? dbContext)
        {
			if (dbContext is null)
			{
				return;
			}


			var entries = dbContext.ChangeTracker.Entries<IBaseAuditableEntity>()
				.Where(entry => entry.State is EntityState.Added or EntityState.Modified);

			foreach (var entry in entries)
			{
				//if(entry.Entity is Order or OrderItem)
				//{
				//	_loggedInUserService.UserId = "";
				//}

				if (entry.State is EntityState.Added)
				{
					entry.Entity.CreatedBy = _loggedInUserService.UserId!;
					entry.Entity.CreatedOn = DateTime.UtcNow;
				}

				entry.Entity.LastModifiedBy = _loggedInUserService.UserId!;
				entry.Entity.LastModifiedOn = DateTime.UtcNow;
			}
		}


		//private void SetPropertyIfExists(object entity, string propertyName, object value)
		//{
		//	var property = entity.GetType().GetProperty(propertyName);

		//	if (property != null && property.CanWrite)
		//	{
		//		property.SetValue(entity, value);
		//	}
		//}


	}
}
