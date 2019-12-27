
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LookDAL.General.IFace
{
    interface ISimpanAsync
    {
        Task<List<T>> SimpanAsync<T>(List<T> listEntity) where T : class;
        Task<T> SimpanAsync<T>(T entity) where T : class;

    }
}
