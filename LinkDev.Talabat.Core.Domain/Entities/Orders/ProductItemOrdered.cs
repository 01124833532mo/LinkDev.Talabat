using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Entities.Orders
{
	public class ProductItemOrdered
	{
        //public ProductItemOrdered()
        //{
            
        //}
        //public ProductItemOrdered(int productid,string productname,string pictureurl)
        //{
        //    ProductId = productid;
        //    ProductName = productname;
        //    PictureUrl = pictureurl;
        //}
        public int ProductId { get; set; }

		public required string ProductName { get; set; }

		public required string PictureUrl { get; set; }

    }
}
