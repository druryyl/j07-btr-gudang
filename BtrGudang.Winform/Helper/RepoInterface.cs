using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtrGudang.Winform.Helper
{
    public interface ISaveChange<T>
    {
        void SaveChanges(T model);
    }
    public interface IDeleteEntity<TKey>
    {
        void DeleteEntity(TKey key);
    }

    public interface ILoadEntity<TModel, TKey>
    {
        TModel LoadEntity(TKey key);
    }

}
