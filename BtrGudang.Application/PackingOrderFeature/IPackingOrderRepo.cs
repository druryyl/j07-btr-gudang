using BtrGudang.Domain.PackingOrderFeature;
using BtrGudang.Helper.Common;

namespace BtrGudang.Winform.Application
{
    public interface IPackingOrderRepo :
        ISaveChange<PackingOrderModel>,
        IDeleteEntity<IPackingOrderKey>,
        ILoadEntity<PackingOrderModel, IPackingOrderKey>
    {
    }
}
