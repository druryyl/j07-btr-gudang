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
        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }

        // Flattened QtyType properties (QtyKecil)
        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public string DepoId { get; set; }

        public static PackingOrderItemDto FromModel(PackingOrderItemModel model, string packingorderId)
        {
            return new PackingOrderItemDto
            {
                PackingOrderId = packingorderId,
                NoUrut = model.NoUrut,
                BrgId = model.Brg.BrgId,
                BrgCode = model.Brg.BrgCode,
                BrgName = model.Brg.BrgName,
                QtyBesar = model.QtyBesar.Qty,
                SatBesar = model.QtyBesar.Satuan,
                QtyKecil = model.QtyKecil.Qty,
                SatKecil = model.QtyKecil.Satuan,
                DepoId   = model.DepoId
            };
        }

        public PackingOrderItemModel ToModel()
        {
            var brg = new BrgType(BrgId, BrgCode, BrgName);
            var qtyBesar = new QtyType(QtyBesar, SatBesar);
            var qtyKecil = new QtyType(QtyKecil, SatKecil);

            return new PackingOrderItemModel(NoUrut, brg, qtyBesar, qtyKecil, DepoId);
        }
    }
}
