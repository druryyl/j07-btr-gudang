using BtrGudang.Winform.Application;
using BtrGudang.Winform.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtrGudang.Winform.Infrastructure
{
    public class DeliveryOrderRepo : IDeliveryOrderRepo
    {
        public void SaveChanges(DeliveryOrderModel model)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(IDeliveryOrderKey key)
        {
            throw new NotImplementedException();
        }

        public DeliveryOrderModel LoadEntity(IDeliveryOrderKey key)
        {
            throw new NotImplementedException();
        }
    }
}
