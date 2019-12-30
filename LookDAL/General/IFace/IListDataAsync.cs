
using EFHelper;
using GenHelper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LookDAL.General.IFace
{
    interface IListDataAsync
    {
        Task<List<TResult>> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn="", bool isascending=false, int toptake=100) 
            where TSource : class where TResult : class;

    }
}
