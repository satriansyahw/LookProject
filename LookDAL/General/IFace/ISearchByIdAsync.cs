
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LookDAL.General.IFace
{
    interface ISearchByIdAsync
    {
        Task<T> SearchByIdAsync<T>(int Id) where T : class;
        Task<List<T>> SearchByIdAsync<T>(List<int> listId) where T : class;

    }
}
