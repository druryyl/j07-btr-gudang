using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtrGudang.Winform.Domain
{
    public class DeliveryOrderModel : IDeliveryOrderKey
    {
        private readonly List<DeliveryOrderItemModel> _listItem;

        public DeliveryOrderModel(
            string deliveryOrderId,
            DateTime deliveryOrderDate,
            string deliveryOrderCode,
            CustomerType customer,
            FakturType faktur,
            LocationType location,
            IEnumerable<DeliveryOrderItemModel> listItem)
        {
            DeliveryOrderId = deliveryOrderId;
            DeliveryOrderDate = deliveryOrderDate;
            DeliveryOrderCode = deliveryOrderCode;
            Customer = customer;
            Faktur = faktur;
            Location = location;
            _listItem = listItem.ToList();
        }

        public static DeliveryOrderModel Default => new DeliveryOrderModel(
            "-",
            new DateTime(3000, 1, 1),
            "-",
            CustomerType.Default,
            FakturType.Default,
            LocationType.Default,
            Enumerable.Empty<DeliveryOrderItemModel>());

        public static IDeliveryOrderKey Key(string id)
        {
            var result = Default;
            result.DeliveryOrderId = id;
            return result;
        }

        public string DeliveryOrderId { get; private set; }
        public DateTime DeliveryOrderDate { get; private set; }
        public string DeliveryOrderCode { get; private set; }
        public CustomerType Customer { get; private set; }
        public FakturType Faktur { get; private set; }
        public LocationType Location { get; private set; }
        public IEnumerable<DeliveryOrderItemModel> ListItem => _listItem;
    }

    public interface IDeliveryOrderKey
    {
        string DeliveryOrderId { get; }
    }
}
