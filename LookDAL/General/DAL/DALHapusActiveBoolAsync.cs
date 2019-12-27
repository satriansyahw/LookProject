using EFHelper.Helper;
using LookDAL.General.IFace;
using LookDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LookDAL.General.DAL
{
    public class DALHapusActiveBoolAsync : IHapusActiveBoolAsync
    {
        RepoGen repo = new RepoGen(new LookDBContext());

        public virtual  async Task<bool> HapusActiveBoolAsync<T>(int identityID, string picDelete) where T : class
        {
            return await repo.DeleteActiveBoolAsync<T>(identityID, picDelete);
        }

        public virtual async Task<bool> HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) where T : class
        {
            return await repo.DeleteActiveBoolAsync<T>(listIdentityID, picDelete);
        }
    }
}
