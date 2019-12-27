
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LookDAL.General.IFace
{
    interface IHapusActiveBoolAsync
    {
        Task<bool> HapusActiveBoolAsync<T>(int identityID,string picDelete) where T : class;
        Task<bool>  HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) where T : class;
    }
}
