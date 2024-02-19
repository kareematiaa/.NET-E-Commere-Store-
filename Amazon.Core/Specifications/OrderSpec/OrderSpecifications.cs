using Amazon.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specifications.OrderSpec
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        public OrderSpecifications(string email)
            :base(O => O.BuyerEmail == email)
        {
            Includes.Add(O =>  O.DeliveryMethod);
            Includes.Add(O => O.Items);                        // for all orders

            AddOrederByDesc(O => O.OrderDate);
        }

        public OrderSpecifications(string email,int orderId)
              : base(O => O.BuyerEmail == email && O.Id == orderId)
        {
            Includes.Add(O => O.DeliveryMethod);            // forspecific order with id
            Includes.Add(O => O.Items);

           
        }
    }
}
