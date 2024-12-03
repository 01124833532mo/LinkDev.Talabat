using LinkDev.Talabat.Core.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts.Specifications.Orders
{
	public class OrderSpecifications : BaseSpecifications<Order,int>
	{
		public OrderSpecifications(string buyerEmail , int orderid) : base(order => order.Id==orderid && order.BuyerEmail == buyerEmail)
		{
			AddIncludes();
		}

		public OrderSpecifications(string buyerEmail) : base(order=>order.BuyerEmail==buyerEmail)
        {
			AddIncludes();
			AddOrderByDesc(order => order.OrderDate);
        }

		private protected override void AddIncludes()
		{
			base.AddIncludes();
			Includes.Add(order => order.DeliveryMethod!);
			Includes.Add(order => order.Items);
		}

	}
}
