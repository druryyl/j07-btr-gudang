using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtrGudang.Winform.Helper
{
    public interface IInsert<T>
    {
        void Insert(T model);
    }

    public interface IUpdate<T>
    {
        void Update(T model);
    }
    public interface IDelete<T>
    {
        void Delete(T key);
    }
    public interface IGetData<TResult, TKey>
    {
        TResult GetData(TKey key);
    }
    public interface IListData<TResult>
    {
        IEnumerable<TResult> ListData();
    }
    public interface IListData<TResult, T>
    {
        IEnumerable<TResult> ListData(T filter);
    }
    public interface IListData<TResult, T1, T2>
    {
        IEnumerable<TResult> ListData(T1 filter1, T2 filter2);
    }
    public interface IListData<TResult, T1, T2, T3>
    {
        IEnumerable<TResult> ListData(T1 filter1, T2 filter2, T3 filter3);
    }

}
