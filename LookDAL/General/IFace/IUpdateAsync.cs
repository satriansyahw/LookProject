
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LookDAL.General.IFace
{
    interface IUpdateAsync
    {
        Task<bool> UbahAsync<T>(T entity) where T : class;
        Task<bool> UbahAsync<T>(List<T> listEntity) where T : class;
    }

}
