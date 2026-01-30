using BtrGudang.Winform.Domain;
using BtrGudang.Winform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtrGudang.Winform.Application
{
    public interface IDeliveryOrderRepo :
        ISaveChange<DeliveryOrderModel>,
        IDeleteEntity<IDeliveryOrderKey>,
        ILoadEntity<DeliveryOrderModel, IDeliveryOrderKey>
    {
    }
}
