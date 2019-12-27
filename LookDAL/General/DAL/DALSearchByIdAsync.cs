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
    public class DALSearchByIdAsync : ISearchByIdAsync
    {
        RepoGen repo = new RepoGen(new LookDBContext());
        public virtual async Task<T> SearchByIdAsync<T>(int Id) where T : class
        {
            return await repo.ListByIDAsync<T>(Id);
        }

        public virtual async Task<List<T>> SearchByIdAsync<T>(List<int> listId) where T : class
        {
            var resultCheck = await repo.ListByIDAsync<T>(listId);
            if (resultCheck != null)
                return resultCheck.ToList();
            else
                return new List<T>();
        }
    }
}
