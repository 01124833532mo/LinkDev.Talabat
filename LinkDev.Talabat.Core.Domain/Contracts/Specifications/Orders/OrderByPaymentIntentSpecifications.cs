using LinkDev.Talabat.Core.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts.Specifications.Orders
{
	public class OrderByPaymentIntentSpecifications : BaseSpecifications<Order,int>
	{

        public OrderByPaymentIntentSpecifications(string paymentIntent) 
            : base(order=>order.PaymentIntentId==paymentIntent)
        {
            
        }
    }
}
