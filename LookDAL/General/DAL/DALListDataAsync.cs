using EFHelper;
using EFHelper.Helper;
using LookDAL.General.IFace;
using LookDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace LookDAL.General.DAL
{
    public class DALListDataAsync : IListDataAsync
    {
        RepoGen repo = new RepoGen(new LookDBContext());

        public virtual async Task<List<TResult>> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn = "", bool isascending = false, int toptake = 100)
            where TSource : class
            where TResult : class
        {
            var sourceType = typeof(TSource);
            var resultType = typeof(TResult);
            if(sourceType == resultType)
            {
                var resultCheck = await repo.ListAsync<TResult>(SearchFieldList, sortColumn, isascending, toptake);
                if (resultCheck != null)
                    return resultCheck.ToList();
                else
                    return new List<TResult>();
            }
            else
            {
                var resultCheck = await repo.ListTResultAsync<TSource, TResult>(SearchFieldList, sortColumn, isascending, toptake);
                if (resultCheck != null)
                    return resultCheck.ToList();
                else
                    return new List<TResult>();
            }
        }

           
    }
}
