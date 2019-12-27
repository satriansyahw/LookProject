
using EFHelper.Helper;
using LookDAL.General.IFace;
using LookDB;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace LookDAL.General.DAL
{
    public class DALSimpanAsync:ISimpanAsync
    {
        RepoGen repo = new RepoGen(new LookDBContext());
        public DALSimpanAsync()
        {

        }

        public virtual async Task<List<T>> SimpanAsync<T>(List<T> listEntity) where T : class
        {
            var result = await repo.SaveAsync<T>(listEntity);
            if (result != null)
                return result.ToList();
            else
                return new List<T>();
        }

        public virtual async Task<T> SimpanAsync<T>(T entity) where T : class
        {
            var result = await repo.SaveAsync<T>(entity);
            return result;
        }
    }

}
