using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtrGudang.Domain.PackingOrderFeature
{
    public class PackingOrderModel : IPackingOrderKey
    {
        private readonly List<PackingOrderItemModel> _listItem;

        public PackingOrderModel(
            string packingOrderId,
            DateTime packingOrderDate,
            string packingOrderCode,
            CustomerType customer,
            FakturType faktur,
            LocationType location,
            IEnumerable<PackingOrderItemModel> listItem)
        {
            PackingOrderId = packingOrderId;
            PackingOrderDate = packingOrderDate;
            PackingOrderCode = packingOrderCode;
            Customer = customer;
            Faktur = faktur;
            Location = location;
            _listItem = listItem.ToList();
        }

        public static PackingOrderModel Default => new PackingOrderModel(
            "-",
            new DateTime(3000, 1, 1),
            "-",
            CustomerType.Default,
            FakturType.Default,
            LocationType.Default,
            Enumerable.Empty<PackingOrderItemModel>());

        public static IPackingOrderKey Key(string id)
        {
            var result = Default;
            result.PackingOrderId = id;
            return result;
        }

        public string PackingOrderId { get; private set; }
        public DateTime PackingOrderDate { get; private set; }
        public string PackingOrderCode { get; private set; }
        public CustomerType Customer { get; private set; }
        public FakturType Faktur { get; private set; }
        public LocationType Location { get; private set; }
        public IEnumerable<PackingOrderItemModel> ListItem => _listItem;
    }

    public interface IPackingOrderKey
    {
        string PackingOrderId { get; }
    }
}
