using BtrGudang.Domain.PackingOrderFeature;
using BtrGudang.Winform.Domain;

namespace BtrGudang.Infrastructure.PackingOrderFeature
{
    public class PackingOrderItemDto
    {
        public string PackingOrderId { get; set; }
        public int NoUrut { get; set; }

        // Flattened BrgType properties
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }

        // Flattened QtyType properties (QtyBesar)
        public decimal QtyBesarQty { get; set; }
        public string QtyBesarSatuan { get; set; }

        // Flattened QtyType properties (QtyKecil)
        public decimal QtyKecilQty { get; set; }
        public string QtyKecilSatuan { get; set; }

        public static PackingOrderItemDto FromModel(PackingOrderItemModel model, string packingorderId)
        {
            return new PackingOrderItemDto
            {
                PackingOrderId = packingorderId,
                NoUrut = model.NoUrut,
                BrgId = model.Brg.BrgId,
                BrgCode = model.Brg.BrgCode,
                BrgName = model.Brg.BrgName,
                QtyBesarQty = model.QtyBesar.Qty,
                QtyBesarSatuan = model.QtyBesar.Satuan,
                QtyKecilQty = model.QtyKecil.Qty,
                QtyKecilSatuan = model.QtyKecil.Satuan
            };
        }

        public PackingOrderItemModel ToModel()
        {
            var brg = new BrgType(BrgId, BrgCode, BrgName);
            var qtyBesar = new QtyType(QtyBesarQty, QtyBesarSatuan);
            var qtyKecil = new QtyType(QtyKecilQty, QtyKecilSatuan);

            return new PackingOrderItemModel(NoUrut, brg, qtyBesar, qtyKecil);
        }
    }
}
