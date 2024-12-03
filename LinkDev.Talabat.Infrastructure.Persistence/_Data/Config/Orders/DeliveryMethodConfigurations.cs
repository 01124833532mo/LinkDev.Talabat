using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data.Config.Orders
{
	internal class DeliveryMethodConfigurations : BaseEntityConfigurations<DeliveryMethod,int>
	{
		public override void Configure(EntityTypeBuilder<DeliveryMethod> builder)
		{
			base.Configure(builder);

			builder.Property(method => method.Cost)
				.HasColumnType("decimal(8,2)");
		}

	}
}
