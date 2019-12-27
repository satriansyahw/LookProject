
using EFHelper.Helper;
using LookDAL.General.IFace;
using LookDB;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace LookDAL.General.DAL
{
    public class DALUpdateAsync : IUpdateAsync
    {
        RepoGen repo = new RepoGen(new LookDBContext());
        public DALUpdateAsync()
        {

        }

        public virtual async Task<bool> UbahAsync<T>(T entity) where T : class
        {
            var result = await repo.UpdateAsync<T>(entity);
            return result;
        }

        public virtual async Task<bool> UbahAsync<T>(List<T> listEntity) where T : class
        {
            var result = await repo.UpdateAsync<T>(listEntity);
            return result;
        }
    }

}
